namespace DocCheck_BallJoint
{
using Elintria;

public class Shoulder : ElintriaScript
{
    public GameObject torso;

    public override void Start()
    {
        BallJoint b = gameObject.AddComponent<BallJoint>();
        if (torso != null) b.connectedBody = torso.GetComponent<Rigidbody>();
        b.axis = Vector3.down;
        b.swingLimit = 70f;
    }
}
}
