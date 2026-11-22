namespace DocCheck_StdModifiers
{
using Elintria;
using Elintria.Standard;

public class PunchAttack : ElintriaScript
{
    public float baseDamage = 10f;
    public float reach = 1.5f;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.forward, out hit, reach)) return;
        Health h = hit.collider.gameObject.GetComponent<Health>();
        if (h != null) h.Damage(baseDamage * StdModifiers.DamageMultiplier(gameObject), gameObject);
    }
}
}
