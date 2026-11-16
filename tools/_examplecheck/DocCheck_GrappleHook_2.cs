namespace DocCheck_GrappleHook
{
using Elintria;
using Elintria.Standard;

public class GrappleStamina : ElintriaScript
{
    public override void Update()
    {
        GrappleHook g = GetComponent<GrappleHook>();
        if (g.pulling && !StdMotor.Spend(gameObject, 10f * Time.deltaTime)) g.Release();
    }
}
}
