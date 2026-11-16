namespace DocCheck_Breakable
{
using Elintria;
using Elintria.Standard;

public class ExplosiveBarrel : ElintriaScript
{
    public void OnDeath()
    {
        foreach (Health h in FindObjectsOfType<Health>())
            if (h.gameObject != gameObject && Vector3.Distance(h.transform.position, transform.position) < 4f)
                StdDamage.Apply(h.gameObject, 50f, gameObject, transform.position, h.transform.position - transform.position, 8f, DamageType.Explosive);
        StdCameraShake.Shake(0.3f, 0.5f);
    }
}
}
