namespace DocCheck_XpReward
{
using Elintria;
using Elintria.Standard;

public class BossReward : ElintriaScript
{
    public override void Start()
    {
        XpReward r = GetComponent<XpReward>();
        r.xp = 500;
        r.money = 200;
    }
}
}
