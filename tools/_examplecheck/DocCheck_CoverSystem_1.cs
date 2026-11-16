namespace DocCheck_CoverSystem
{
using Elintria;
using Elintria.Standard;

public class CoverHides : ElintriaScript
{
    public override void Update()
    {
        CoverSystem c = GetComponent<CoverSystem>();
        StealthPlayer s = GetComponent<StealthPlayer>();
        if (s != null) s.hidden = c.inCover && c.lowCover && !c.peeking;
    }
}
}
