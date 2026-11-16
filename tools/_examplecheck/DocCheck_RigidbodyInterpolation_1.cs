namespace DocCheck_RigidbodyInterpolation
{
using Elintria;

public class InterpReport : ElintriaScript
{
    public override void Start() { Debug.Log(name + " interpolation: " + GetComponent<Rigidbody>().interpolation); }
}
}
