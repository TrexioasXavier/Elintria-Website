namespace DocCheck_Water
{
using Elintria;

public class WaveRider : ElintriaScript
{
    public float draft = 0.3f;

    public override void Update()
    {
        if (!Water.SurfaceAt(transform.position, out float h, out Vector3 n)) return;
        Vector3 p = transform.position;
        p.y = h - draft;
        transform.position = p;
        Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, n).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(fwd, n), 3f * Time.deltaTime);
    }
}
}
