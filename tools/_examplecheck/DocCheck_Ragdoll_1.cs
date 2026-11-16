namespace DocCheck_Ragdoll
{
using Elintria;
using Elintria.Standard;

public class DeathPush : ElintriaScript
{
    public override void Start()
    {
        GetComponent<Ragdoll>().ragdollOnDeath = true;
    }

    public void OnDeath()
    {
        Health h = GetComponent<Health>();
        if (h.lastAttacker != null)
            GetComponent<Ragdoll>().AddImpulse((transform.position - h.lastAttacker.transform.position).normalized * 6f, transform.position + Vector3.up);
    }
}
}
