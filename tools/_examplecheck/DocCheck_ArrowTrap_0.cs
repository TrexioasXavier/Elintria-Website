namespace DocCheck_ArrowTrap
{
using Elintria;
using Elintria.Standard;

public class PoisonDarts : ElintriaScript
{
    public override void Start()
    {
        ArrowTrap t = GetComponent<ArrowTrap>();
        t.damageType = (int)DamageType.Poison;
        t.damage = 4f;
        t.interval = 1f;
    }
}
}
