namespace DocCheck_MeshCollider
{
using Elintria;

public class SurfaceInfo : ElintriaScript
{
    public override void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 3f) && hit.collider is MeshCollider)
            ScreenGUI.Text(20, 20, "Slope: " + Vector3.Angle(hit.normal, Vector3.up).ToString("0") + " deg on " + hit.gameObject.name);
    }
}
}
