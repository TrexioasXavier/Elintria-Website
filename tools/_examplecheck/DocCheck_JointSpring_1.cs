namespace DocCheck_JointSpring
{
using Elintria;

public class HoldLever : ElintriaScript
{
    public float hold = 30f;

    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.useSpring = true;
        h.spring = new JointSpring { spring = 80f, damper = 8f, targetPosition = hold };
    }
}
}
