namespace DocCheck_JointSpring
{
using Elintria;

public class StiffenSpring : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Alpha1)) return;
        HingeJoint h = GetComponent<HingeJoint>();
        JointSpring s = h.spring;
        s.spring *= 2f;
        h.spring = s;
    }
}
}
