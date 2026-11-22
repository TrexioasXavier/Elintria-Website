namespace DocCheck_JointMotor
{
using Elintria;

public class SlowFan : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.forward;
        h.useMotor = true;
        h.motor = new JointMotor { targetVelocity = 90f, force = 20f };
    }
}
}
