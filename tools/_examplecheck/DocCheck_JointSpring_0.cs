namespace DocCheck_JointSpring
{
using Elintria;

public class SpringFlap : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.right;
        h.useSpring = true;
        h.spring = new JointSpring { spring = 15f, damper = 1f, targetPosition = 0f };
    }
}
}
