namespace DocCheck_StdMotor
{
using Elintria;
using Elintria.Standard;

public class JumpPad : ElintriaScript
{
    public float power = 14f;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || Vector3.Distance(p.transform.position, transform.position) > 1.2f) return;
        Vector3 v = StdMotor.GetVelocity(p);
        v.y = power;
        StdMotor.SetVelocity(p, v);
    }
}
}
