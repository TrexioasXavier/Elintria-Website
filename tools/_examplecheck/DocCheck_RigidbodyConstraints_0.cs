namespace DocCheck_RigidbodyConstraints
{
using Elintria;

public class SideScroller : ElintriaScript
{
    public override void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }
}
}
