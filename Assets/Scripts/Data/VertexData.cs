using System.Collections.Generic;
using UnityEngine;

public class VertexData
{
    public Vector3 worldPosition;
    public List<VertexData> neighbors;
    public int index;

    public VertexData(Vector3 worldPos, int idx)
    {
        worldPosition = worldPos;
        neighbors = new List<VertexData>();
        index = idx;
    }

    public void findNeighbour(List<VertexData> vertices, float _epsilon)
    {
        foreach (VertexData vd in vertices)
        {
            if (CheckDistance(this, vd))
            {
                neighbors.Add(vd);
            }
        }
    }

    public static bool CheckDistance(VertexData origin, VertexData other)
    {
        return Vector3.Distance(origin.worldPosition, other.worldPosition)
            < (PaintOnGeneratedTexture.brushSize / 100f);
    }
}
