namespace DocCheck_RigidbodyConstraints
{
using Elintria;

public class UprightBody : ElintriaScript
{
    public override void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;   // may still turn about Y
    }
}
}
