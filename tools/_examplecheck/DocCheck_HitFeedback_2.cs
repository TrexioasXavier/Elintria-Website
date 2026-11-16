namespace DocCheck_HitFeedback
{
using Elintria;
using Elintria.Standard;

public class InCombat : ElintriaScript
{
    public override void Update()
    {
        HitFeedback h = GetComponent<HitFeedback>();
        bool fighting = Time.time - h.lastHitTime < 5f;
        if (fighting) ScreenGUI.Text(20f, 200f, "In combat", null, new Color(1f, 0.4f, 0.4f, 1f));
    }
}
}
