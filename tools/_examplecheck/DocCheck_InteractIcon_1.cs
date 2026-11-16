namespace DocCheck_InteractIcon
{
using Elintria;
using Elintria.Standard;

public class IconsInCombat : ElintriaScript
{
    public override void Update()
    {
        HitFeedback h = GetComponent<HitFeedback>();
        GetComponent<InteractIcon>().enabled = h == null || Time.time - h.lastHitTime > 3f;
    }
}
}
