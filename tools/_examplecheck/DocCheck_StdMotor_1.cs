namespace DocCheck_StdMotor
{
using Elintria;
using Elintria.Standard;

public class Swamp : ElintriaScript
{
    public float radius = 6f;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null) return;
        bool inside = Vector3.Distance(p.transform.position, transform.position) < radius;
        StdMotor.SetSpeedFactor(p, "swamp", inside ? 0.5f : 1f);
    }
}
}
