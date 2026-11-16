namespace DocCheck_Destructible
{
using Elintria;
using Elintria.Standard;

public class BreakableWall : ElintriaScript
{
    public override void OnCollisionEnter(Collision c)
    {
        if (c.relativeVelocity.magnitude > 6f && c.gameObject.CompareTag("Ram"))
            GetComponent<Destructible>().BreakFrom(c.relativeVelocity.normalized);
    }
}
}
