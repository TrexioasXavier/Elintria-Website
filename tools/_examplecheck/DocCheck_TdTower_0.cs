namespace DocCheck_TdTower
{
using Elintria;
using Elintria.Standard;

public class PoisonTower : ElintriaScript
{
    public override void Awake()
    {
        TdTower t = GetComponent<TdTower>();
        t.towerName = "Poison";
        t.cost = 80;
        t.damage = 3f;
        t.damageType = 2;        // poison keeps hurting
        t.splashRadius = 1.5f;
    }
}
}
