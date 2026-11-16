namespace DocCheck_StdDamage
{
using Elintria;
using Elintria.Standard;

public class LandingDamage : ElintriaScript
{
    public override void OnCollisionEnter(Collision c)
    {
        float speed = c.relativeVelocity.magnitude;
        if (speed > 12f) StdDamage.Apply(gameObject, (speed - 12f) * 6f, c.gameObject, transform.position, Vector3.down, 0f);
    }
}
}
