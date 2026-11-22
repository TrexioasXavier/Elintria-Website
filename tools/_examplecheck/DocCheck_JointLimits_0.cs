namespace DocCheck_JointLimits
{
using Elintria;

public class OneWayDoor : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.useLimits = true;
        h.limits = new JointLimits(0f, 110f);
    }
}
}
