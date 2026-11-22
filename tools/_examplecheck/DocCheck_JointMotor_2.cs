namespace DocCheck_JointMotor
{
using Elintria;

public class WeakWinch : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.useMotor = true;
        h.motor = new JointMotor { targetVelocity = 60f, force = 2f };   // stalls under weight
    }
}
}
