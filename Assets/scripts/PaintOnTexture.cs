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
    private Mesh mesh;
    private Color[] vertexColors;

    private const float epsilon = 0.001f;

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

    void Update() { }

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
            vd.findNeighbour(vertexDataList, brushSize / 100f);
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
            if (AreVectorsEqual(vd.worldPosition, vtx))
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
        }

        mesh.colors = vertexColors;
    }

    public void EraseAtUV(Vector2 uv)
    {
        // int centerX = (int)(uv.x * maskTexture.width);
        // int centerY = (int)(uv.y * maskTexture.height);
        // int radius = Mathf.FloorToInt(brushSize / 2);

        // for (int i = -radius; i <= radius; i++)
        // {
        //     for (int j = -radius; j <= radius; j++)
        //     {
        //         float distance = Mathf.Sqrt(i * i + j * j) / radius;
        //         if (distance <= 1f)
        //         {
        //             float intensity = Mathf.Pow(1f - distance, 2);
        //             int targetX = Mathf.Clamp(centerX + i, 0, maskTexture.width - 1);
        //             int targetY = Mathf.Clamp(centerY + j, 0, maskTexture.height - 1);

        //             Color targetColor = maskTexture.GetPixel(targetX, targetY);
        //             targetColor.a = Mathf.Clamp01(targetColor.a - alphaIncrease * intensity);
        //             maskTexture.SetPixel(targetX, targetY, targetColor);
        //         }
        //     }
        // }
        // maskTexture.Apply();
        // material.SetTexture("_Mask", maskTexture);
    }

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
                localPosition = vd.worldPosition,
                index = vd.index,
                neighbors = vd.neighbors.Select(n => n.index).ToList(),
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
        }
    }
}
