namespace DocCheck_RigidbodyInterpolation
{
using Elintria;

public class SmoothBody : ElintriaScript
{
    public override void Start() { GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate; }
}
}
