using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VertexDataSerializable
{
    public Vector3 localPosition;
    public int index;
    public List<int> neighbors;
    public int closestNeighborBelow;
}
