namespace DocCheck_RigidbodyConstraints
{
using Elintria;

public class Freezer : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = rb.constraints == RigidbodyConstraints.FreezeAll ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
    }
}
}
