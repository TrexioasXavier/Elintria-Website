namespace DocCheck_DashAbility
{
using Elintria;
using Elintria.Standard;

public class DashDodge : ElintriaScript
{
    public override void Update()
    {
        Health h = GetComponent<Health>();
        h.invulnerableTime = GetComponent<DashAbility>().dashing ? 0.3f : 0f;
    }
}
}
