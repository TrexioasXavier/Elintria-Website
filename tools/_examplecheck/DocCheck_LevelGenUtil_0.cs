namespace DocCheck_LevelGenUtil
{
using Elintria;

public class NodeEquivalent : ElintriaScript
{
    public override void Start()
    {
        LevelGenerator.Generate("warehouse", "seed=9");
        int placed = LevelGenUtil.Place("Enemy", "Template_Robot");
        Debug.Log(placed + " of " + LevelGenUtil.Count("Enemy") + " enemy spots filled");
    }
}
}
