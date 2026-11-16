namespace DocCheck_GpuMesh
{
using Elintria;

public class GpuTriangle : ElintriaScript
{
    GpuMesh mesh;
    MeshInstance inst;

    public override void Start()
    {
        mesh = new GpuMesh();
        float[] pos = { 0, 2, 0,  -1, 0, 0,  1, 0, 0 };
        uint[] col = { 0xFF0000FF, 0xFF00FF00, 0xFFFF0000 };   // red, green, blue (0xAABBGGRR)
        mesh.SetData(pos, null, col, null, 3, new[] { 0, 1, 2 }, 3);
        inst = new MeshInstance(mesh);
        inst.SetPosition(0f, 1f, 5f);
        inst.SetMaterial(0, 1f, 1f, 1f, 1f, MaterialFlags.DoubleSided);
    }

    public override void OnDestroy() { inst.Destroy(); mesh.Release(); }
}
}
