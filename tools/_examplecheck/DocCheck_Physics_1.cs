namespace DocCheck_Physics
{
using Elintria;

public class Grenade : ElintriaScript
{
    public float fuse = 3f;
    public float radius = 6f;
    public float damage = 80f;

    public override void Start() { Invoke("Explode", fuse); }

    void Explode()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, radius))
        {
            GameObject g = c.gameObject;
            if (g == gameObject) continue;
            Vector3 target = c.bounds.center;
            bool covered = Physics.Linecast(transform.position + Vector3.up * 0.2f, target, out RaycastHit block) && block.gameObject != g;
            if (covered) continue;
            float falloff = 1f - Vector3.Distance(transform.position, target) / radius;
            g.SendMessage("ApplyDamage", damage * Mathf.Clamp01(falloff));
            Rigidbody rb = g.GetComponent<Rigidbody>();
            if (rb != null) rb.AddExplosionForce(700f, transform.position, radius);
        }
        Destroy(gameObject);
    }
}
}
