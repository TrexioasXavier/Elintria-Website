namespace DocCheck_ContactPoint
{
using Elintria;

public class StandingOn : ElintriaScript
{
    public override void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint p in collision.contacts)
            if (p.normal.y > 0.7f) { ScreenGUI.Text(20, 60, "Standing on " + p.otherCollider.gameObject.name); break; }
    }
}
}
