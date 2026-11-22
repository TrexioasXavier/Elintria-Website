namespace DocCheck_HingeJoint
{
using Elintria;

public class SwingDoor : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.anchor = new Vector3(-0.5f, 0f, 0f);       // the edge with the hinges
        h.axis = Vector3.up;
        h.useLimits = true;
        h.limits = new JointLimits(-100f, 100f);
    }
}
}
