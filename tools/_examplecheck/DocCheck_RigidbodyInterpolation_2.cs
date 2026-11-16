namespace DocCheck_RigidbodyInterpolation
{
using Elintria;

public class Teleporter : ElintriaScript
{
    public void TeleportBody(Rigidbody rb, Vector3 to)
    {
        RigidbodyInterpolation old = rb.interpolation;
        rb.interpolation = RigidbodyInterpolation.None;
        rb.position = to;
        rb.interpolation = old;
    }
}
}
