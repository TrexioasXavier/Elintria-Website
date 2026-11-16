namespace DocCheck_GrappleHook
{
using Elintria;
using Elintria.Standard;

public class LongHook : ElintriaScript
{
    public override void Start()
    {
        GrappleHook g = GetComponent<GrappleHook>();
        g.range = 50f;
        g.pullSpeed = 25f;
    }
}
}
