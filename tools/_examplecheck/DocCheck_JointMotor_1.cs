namespace DocCheck_JointMotor
{
using Elintria;

public class ReverseMotor : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        HingeJoint h = GetComponent<HingeJoint>();
        JointMotor m = h.motor;
        m.targetVelocity = -m.targetVelocity;
        h.motor = m;
    }
}
}
