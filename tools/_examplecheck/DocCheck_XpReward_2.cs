namespace DocCheck_XpReward
{
using Elintria;
using Elintria.Standard;

public class NoFarming : ElintriaScript
{
    public override void Start()
    {
        if (StdVars.GetInt("farm_kills") > 50) GetComponent<XpReward>().xp = 0;
        StdVars.Add("farm_kills", 1);
    }
}
}
