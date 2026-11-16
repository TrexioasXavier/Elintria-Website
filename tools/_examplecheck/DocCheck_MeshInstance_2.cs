namespace DocCheck_MeshInstance
{
using Elintria;

public class GhostPreview : ElintriaScript
{
    MeshInstance ghost;

    public void Show(GpuMesh mesh, Vector3 at, bool valid)
    {
        if (ghost == null) ghost = new MeshInstance(mesh);
        ghost.SetPosition(at.x, at.y, at.z);
        ghost.SetMaterial(0, valid ? 0.3f : 1f, valid ? 1f : 0.3f, 0.3f, 0.4f, MaterialFlags.Transparent | MaterialFlags.NoDepthWrite);
    }

    public override void OnDestroy() { if (ghost != null) ghost.Destroy(); }
}
}
