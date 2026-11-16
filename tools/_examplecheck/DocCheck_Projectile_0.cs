namespace DocCheck_Projectile
{
using Elintria;
using Elintria.Standard;

public class Homing2 : ElintriaScript
{
    public float turnRate = 90f;

    public override void Update()
    {
        Projectile p = GetComponent<Projectile>();
        GameObject target = GameObject.FindWithTag("Enemy");
        if (p == null || target == null) return;
        Vector3 want = (target.transform.position - transform.position).normalized * p.velocity.magnitude;
        p.velocity = Vector3.RotateTowards(p.velocity, want, turnRate * Mathf.Deg2Rad * Time.deltaTime, 0f);
    }
}
}
