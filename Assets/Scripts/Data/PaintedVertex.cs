using UnityEngine;

public class PaintedVertex
{
    public VertexData vertex;
    public VertexData closestNeighborBelow;

    public PaintedVertex(VertexData vertex, VertexData closestNeighborBelow)
    {
        this.vertex = vertex;
        this.closestNeighborBelow = closestNeighborBelow;
    }
}
