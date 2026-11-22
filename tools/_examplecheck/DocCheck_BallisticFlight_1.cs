namespace DocCheck_BallisticFlight
{
using Elintria;
using Elintria.Standard;

public class FloatPad : ElintriaScript
{
    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || BallisticFlight.IsFlying(p)) return;
        if (Vector3.Distance(p.transform.position, transform.position) < 1.5f)
            BallisticFlight.Launch(p, Vector3.up * 8f, 0.6f, 4f, 0.3f);   // slow, floaty
    }
}
}
