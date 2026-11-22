namespace DocCheck_RopeSwing
{
using Elintria;
using Elintria.Standard;

public class PitRope : ElintriaScript
{
    public override void Start()
    {
        RopeSwing r = GetComponent<RopeSwing>();
        r.length = 9f;
        r.releaseBoost = 5f;
        r.grabReach = 2f;
    }
}
}
