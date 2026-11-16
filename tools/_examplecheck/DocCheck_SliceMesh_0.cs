namespace DocCheck_SliceMesh
{
using Elintria;
using Elintria.Standard;

public class HalveCube : ElintriaScript
{
    public override void Start()
    {
        SliceMesh.Cube().Split(Vector3.zero, new Vector3(1f, 1f, 0f).normalized, out SliceMesh a, out SliceMesh b);
        Show(a, new Vector3(-1f, 1f, 0f));
        Show(b, new Vector3(1f, 1f, 0f));
    }

    static void Show(SliceMesh m, Vector3 at)
    {
        GameObject g = new GameObject("Half");
        g.transform.position = at;
        var rm = new RuntimeMesh();
        rm.SetVertices(m.v.ToArray());
        rm.SetUVs(m.uv.ToArray());
        rm.SetTriangles(m.tri.ToArray());
        RuntimeMesh.Assign(g, rm);
    }
}
}
