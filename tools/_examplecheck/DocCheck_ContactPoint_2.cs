namespace DocCheck_ContactPoint
{
using Elintria;

public class DeepContact : ElintriaScript
{
    public override void OnCollisionStay(Collision collision)
    {
        ContactPoint p = collision.GetContact(0);
        if (p.separation < -0.1f) Debug.LogWarning(p.thisCollider.gameObject.name + " is sinking into " + p.otherCollider.gameObject.name);
    }
}
}
