namespace DocCheck_RuntimeMesh
{
using Elintria;

public class MakeQuad : ElintriaScript
{
    public override void Start()
    {
        RuntimeMesh m = new RuntimeMesh();
        m.SetVertices(new[] { new Vector3(-1, 0, -1), new Vector3(1, 0, -1), new Vector3(1, 0, 1), new Vector3(-1, 0, 1) });
        m.SetNormals(new[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up });
        m.SetUVs(new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) });
        m.SetTriangles(new[] { 0, 2, 1, 0, 3, 2 });   // anticlockwise seen from above
        RuntimeMesh.Assign(gameObject, m);
    }
}
}
