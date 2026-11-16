namespace DocCheck_DamageResistance
{
using Elintria;
using Elintria.Standard;

public class BreakingArmour : ElintriaScript
{
    public void OnDamaged(float amount)
    {
        DamageResistance r = GetComponent<DamageResistance>();
        r.armor = Mathf.Max(0f, r.armor - 1f);
    }
}
}
