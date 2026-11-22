namespace DocCheck_FixedJoint
{
using Elintria;

public class GlueCrates : ElintriaScript
{
    public GameObject other;

    public override void Start()
    {
        if (other == null) return;
        FixedJoint j = gameObject.AddComponent<FixedJoint>();
        j.connectedBody = other.GetComponent<Rigidbody>();
    }
}
}
