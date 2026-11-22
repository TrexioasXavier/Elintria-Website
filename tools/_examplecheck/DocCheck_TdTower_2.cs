namespace DocCheck_TdTower
{
using Elintria;
using Elintria.Standard;

public class VeteranTower : ElintriaScript
{
    public override void Update()
    {
        TdTower t = GetComponent<TdTower>();
        if (t.shots >= 50 * t.level) t.LevelUp();
    }
}
}
