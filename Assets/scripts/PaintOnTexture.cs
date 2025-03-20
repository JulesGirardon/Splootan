using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public static float brushSize = 0.5f;
    public float alphaIncrease = 0.1f;
    public Color paintColor = Color.red;

    private List<VertexData> vertexDataList;
    private List<PaintedVertex> paintedVertices = new List<PaintedVertex>();
    private Mesh mesh;
    private Color[] vertexColors;

    private const float epsilon = 0.001f;
    private float flowPaintTimer = 0f;
    private const float flowPaintInterval = 0.05f;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        string filePath = Path.Combine(
            Application.persistentDataPath,
            "vertexData_" + gameObject.name + ".json"
        );

        if (File.Exists(filePath))
        {
            VertexDataList vertexDataListSerializable = LoadVertexDataFromFile(filePath);
            if (
                vertexDataListSerializable != null
                && Mathf.Approximately(vertexDataListSerializable.brushSize, brushSize)
            )
            {
                LoadVertexData(vertexDataListSerializable);
            }
            else
            {
                InitializeVertexData();
                SaveVertexDataToFile(filePath);
            }
        }
        else
        {
            InitializeVertexData();
            SaveVertexDataToFile(filePath);
        }

        ClearVertices();

        Debug.Log(
            "Nom de la statut : "
                + gameObject.name
                + ". Nombre de vertices : "
                + vertexDataList.Count
        );
    }

    void FixedUpdate()
    {
        flowPaintTimer += Time.deltaTime;
        if (flowPaintTimer >= flowPaintInterval)
        {
            FlowPaint();
            flowPaintTimer = 0f;
        }
    }

    private void InitializeVertexData()
    {
        Vector3[] vertices = mesh.vertices;

        vertexDataList = new List<VertexData>();

        for (int i = 0; i < vertices.Length; i++)
        {
            vertexDataList.Add(new VertexData(vertices[i], i));
        }

        foreach (VertexData vd in vertexDataList)
        {
            vd.findNeighbour(vertexDataList);
        }
    }

    private void ClearVertices()
    {
        vertexColors = new Color[mesh.vertexCount];
        for (int i = 0; i < vertexColors.Length; i++)
        {
            vertexColors[i] = Color.clear;
        }
        mesh.colors = vertexColors;
    }

    private bool AreVectorsEqual(Vector3 v1, Vector3 v2)
    {
        return Mathf.Abs(v1.x - v2.x) < epsilon
            && Mathf.Abs(v1.y - v2.y) < epsilon
            && Mathf.Abs(v1.z - v2.z) < epsilon;
    }

    public VertexData findVertexDataByVertex(Vector3 vtx)
    {
        foreach (VertexData vd in vertexDataList)
        {
            if (AreVectorsEqual(vd.localPosition, vtx))
            {
                return vd;
            }
        }
        return null;
    }

    public void PaintAtVertex(Vector3 vtx)
    {
        VertexData targetVertex = findVertexDataByVertex(vtx);
        if (targetVertex == null)
            return;

        foreach (VertexData vd in targetVertex.neighbors)
        {
            int index = vd.index;
            Color targetColor = vertexColors[index];
            targetColor = Color.Lerp(targetColor, paintColor, alphaIncrease);
            vertexColors[index] = targetColor;

            if (
                vd.closestNeighborBelow != null
                && !paintedVertices.Any(pv =>
                    pv.vertex == vd && pv.closestNeighborBelow == vd.closestNeighborBelow
                )
                && targetColor.a > 0
            )
            {
                paintedVertices.Add(new PaintedVertex(vd, vd.closestNeighborBelow));
            }
        }

        mesh.colors = vertexColors;
    }

    private void FlowPaint()
    {
        List<PaintedVertex> newPaintedVertices = new List<PaintedVertex>();
        Debug.Log("Nombre de vertices peints : " + paintedVertices.Count);

        foreach (PaintedVertex paintedVertex in paintedVertices.ToList())
        {
            VertexData vd = paintedVertex.vertex;
            VertexData neighbor = paintedVertex.closestNeighborBelow;

            if (vd == null || neighbor == null)
            {
                continue;
            }

            Color currentColor = vertexColors[vd.index];
            Color neighborColor = vertexColors[neighbor.index];

            if (currentColor.a > 0)
            {
                float flowAmount = 0.4f;

                if (neighborColor.a < 1f)
                {
                    neighborColor = Color.Lerp(neighborColor, currentColor, flowAmount);
                    vertexColors[neighbor.index] = neighborColor;

                    if (neighborColor.a > 0 && !newPaintedVertices.Any(pv => pv.vertex == neighbor))
                    {
                        newPaintedVertices.Add(
                            new PaintedVertex(neighbor, neighbor.closestNeighborBelow)
                        );
                    }
                }

                currentColor.a = Mathf.Max(
                    Mathf.Clamp01(currentColor.a - flowAmount),
                    currentColor.a
                );
                vertexColors[vd.index] = currentColor;
            }
            paintedVertices.Remove(paintedVertex);
        }

        paintedVertices.AddRange(newPaintedVertices);

        mesh.colors = vertexColors;
    }

    public void EraseAtVertex(Vector2 uv) { }

    public void CalculatePaintArea()
    {
        int paintedVertices = 0;

        foreach (Color vertexColor in vertexColors)
        {
            if (vertexColor.a > 0f)
            {
                paintedVertices++;
            }
        }

        float paintedPercentage = (float)paintedVertices / vertexColors.Length * 100f;
        Debug.Log(
            "Nom de la statut" + gameObject.name + ". Surface peinte : " + paintedPercentage + "%"
        );

        string originalName = gameObject.name;
        string cleanedName = originalName.Replace("(Clone)", "");
        Global.statuePercentage[cleanedName] = paintedPercentage;

        if (paintedPercentage >= 65f)
        {
            Debug.Log("Statue complète ! (" + gameObject.name + ").");
            LaunchStatueFinding();
        }
    }

    private void LaunchStatueFinding()
    {
        GameObject findStatuesObject = GameObject.Find("StatuesManager");
        if (findStatuesObject != null)
        {
            FindStatues findStatuesScript = findStatuesObject.GetComponent<FindStatues>();
            if (findStatuesScript != null)
            {
                findStatuesScript.enabled = true;
            }
            else
            {
                Debug.LogWarning("Le script FindStatues n'a pas été trouvé sur le GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Le GameObject avec le script FindStatues n'a pas été trouvé.");
        }
    }

    private void SaveVertexDataToFile(string filePath)
    {
        List<VertexDataSerializable> vertexDataSerializableList =
            new List<VertexDataSerializable>();

        foreach (VertexData vd in vertexDataList)
        {
            VertexDataSerializable vds = new VertexDataSerializable
            {
                localPosition = vd.localPosition,
                index = vd.index,
                neighbors = vd.neighbors.Select(n => n.index).ToList(),
                closestNeighborBelow =
                    vd.closestNeighborBelow != null ? vd.closestNeighborBelow.index : -1,
            };
            vertexDataSerializableList.Add(vds);
        }

        VertexDataList vertexDataListSerializable = new VertexDataList
        {
            brushSize = brushSize,
            vertices = vertexDataSerializableList,
        };

        string json = JsonUtility.ToJson(vertexDataListSerializable);
        File.WriteAllText(filePath, json);
    }

    private VertexDataList LoadVertexDataFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<VertexDataList>(json);
        }
        else
        {
            Debug.LogWarning("Le fichier de données des sommets n'a pas été trouvé.");
            return null;
        }
    }

    private void LoadVertexData(VertexDataList vertexDataListSerializable)
    {
        vertexDataList = new List<VertexData>();
        foreach (VertexDataSerializable vds in vertexDataListSerializable.vertices)
        {
            VertexData vd = new VertexData(vds.localPosition, vds.index);
            vertexDataList.Add(vd);
        }

        foreach (VertexDataSerializable vds in vertexDataListSerializable.vertices)
        {
            VertexData vd = vertexDataList[vds.index];
            vd.neighbors = vds.neighbors.Select(index => vertexDataList[index]).ToList();
            vd.closestNeighborBelow =
                vds.closestNeighborBelow != -1 ? vertexDataList[vds.closestNeighborBelow] : null;
        }
    }
}
