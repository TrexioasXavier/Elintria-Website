namespace DocCheck_JointLimits
{
using Elintria;

public class LooseHinge : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.L)) return;
        HingeJoint h = GetComponent<HingeJoint>();
        JointLimits l = h.limits;
        l.min -= 20f;
        l.max += 20f;
        h.limits = l;
    }
}
}
