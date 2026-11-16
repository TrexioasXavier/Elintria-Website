namespace DocCheck_XpReward
{
using Elintria;
using Elintria.Standard;

public class RewardByHealth : ElintriaScript
{
    public override void Start()
    {
        XpReward r = gameObject.AddComponent<XpReward>();
        r.xp = (int)(GetComponent<Health>().maxHealth / 4f);
    }
}
}
