namespace DocCheck_HingeJoint
{
using Elintria;

public class MotorWheel : ElintriaScript
{
    public float speed = 180f;

    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.right;
        h.useMotor = true;
        h.motor = new JointMotor { targetVelocity = speed, force = 200f };
    }
}
}
