namespace DocCheck_Collision
{
using Elintria;

public class FallDamage : ElintriaScript
{
    public float safeSpeed = 8f;

    public override void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;
        if (speed > safeSpeed) SendMessage("ApplyDamage", (speed - safeSpeed) * 5f);
    }
}
}
