using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VertexData
{
    public Vector3 worldPosition;
    public List<VertexData> neighbors;
    public int index;
    public VertexData closestNeighborBelow;
    private List<VertexData> closestNeighborsBelow;

    public VertexData(Vector3 worldPos, int idx)
    {
        worldPosition = worldPos;
        neighbors = new List<VertexData>();
        index = idx;
        closestNeighborsBelow = new List<VertexData>();
    }

    public void findNeighbour(List<VertexData> vertices)
    {
        foreach (VertexData vd in vertices)
        {
            if (CheckDistance(this, vd))
            {
                neighbors.Add(vd);
            }
        }

        closestNeighborsBelow.Clear();
        List<KeyValuePair<VertexData, float>> neighborsWithDistance =
            new List<KeyValuePair<VertexData, float>>();

        foreach (VertexData neighbor in neighbors)
        {
            if (neighbor.worldPosition.y < this.worldPosition.y)
            {
                float distance = Vector3.Distance(this.worldPosition, neighbor.worldPosition);
                neighborsWithDistance.Add(new KeyValuePair<VertexData, float>(neighbor, distance));
            }
        }

        closestNeighborsBelow = neighborsWithDistance
            .OrderBy(x => x.Value)
            .Select(x => x.Key)
            .Take(Mathf.Min(3, neighborsWithDistance.Count))
            .ToList();

        if (closestNeighborsBelow.Count > 0)
        {
            closestNeighborBelow = closestNeighborsBelow[
                Random.Range(0, closestNeighborsBelow.Count)
            ];
        }
        else
        {
            closestNeighborBelow = null;
        }
    }

    public static bool CheckDistance(VertexData origin, VertexData other)
    {
        return Vector3.Distance(origin.worldPosition, other.worldPosition)
            < (PaintOnGeneratedTexture.brushSize / 100f);
    }
}
