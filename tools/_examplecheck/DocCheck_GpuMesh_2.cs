namespace DocCheck_GpuMesh
{
using Elintria;

public class ChunkMesh : ElintriaScript
{
    GpuMesh mesh = new GpuMesh();
    MeshInstance inst;

    public void Rebuild(float[] positions, int[] indices)
    {
        mesh.SetData(positions, null, null, null, positions.Length / 3, indices, indices.Length);
        if (inst == null) { inst = new MeshInstance(mesh); inst.SetPosition(0f, 0f, 0f); }
    }

    public override void OnDestroy() { if (inst != null) inst.Destroy(); mesh.Release(); }
}
}
