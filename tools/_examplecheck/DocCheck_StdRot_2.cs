namespace DocCheck_StdRot
{
using Elintria;
using Elintria.Standard;

public class TiltBoat : ElintriaScript
{
    public override void Update()
    {
        if (Water.SurfaceAt(transform.position, out float h, out Vector3 n))
            transform.rotation = StdRot.Clamp(StdRot.FromTo(Vector3.up, n), 20f) * Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }
}
}
