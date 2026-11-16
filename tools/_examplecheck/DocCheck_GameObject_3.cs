namespace DocCheck_GameObject
{
using Elintria;

public class Shockwave : ElintriaScript
{
    public float radius = 6f;
    public float damage = 40f;

    public void Detonate()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, radius))
        {
            GameObject target = c.gameObject;
            if (target == gameObject) continue;
            float falloff = 1f - Vector3.Distance(target.transform.position, transform.position) / radius;
            target.SendMessage("ApplyDamage", damage * Mathf.Clamp01(falloff));
            target.BroadcastMessage("OnShockwave");   // e.g. lamps flicker
        }
        Destroy(gameObject);
    }
}
}
