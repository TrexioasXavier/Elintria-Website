namespace DocCheck_Projectile
{
using Elintria;
using Elintria.Standard;

public class StickyBomb : ElintriaScript
{
    float stuckAt = -1f;

    public override void Update()
    {
        Projectile p = GetComponent<Projectile>();
        if (stuckAt < 0f && Physics.Raycast(transform.position, p.velocity, out RaycastHit hit, p.velocity.magnitude * Time.deltaTime + 0.1f))
        {
            p.velocity = Vector3.zero;
            p.gravity = 0f;
            transform.position = hit.point;
            stuckAt = Time.time;
        }
        if (stuckAt >= 0f && Time.time - stuckAt > 2f && !p.exploded) p.Explode(null, transform.position, Vector3.up);
    }
}
}
