namespace DocCheck_StdEvents
{
using Elintria;
using Elintria.Standard;

public class DamageTally : ElintriaScript
{
    public float dealt;

    public override void Start()
    {
        StdEvents.Damaged += (h, amount, attacker) =>
        {
            if (attacker != gameObject) return;
            dealt += amount;
            if (dealt >= 1000f) StdToast.Show("1000 damage dealt!");
        };
    }
}
}
