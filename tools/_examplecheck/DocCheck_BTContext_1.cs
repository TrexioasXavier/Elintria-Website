namespace DocCheck_BTContext
{
using Elintria;
using Elintria.AI;

public class Cooldowns : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Brute.elbt"); }

    bool Ready(BTContext c, string key) => c.time >= c.blackboard.Float(key + "Ready");

    BTStatus Slam(BTContext c)
    {
        if (!Ready(c, "slam")) return BTStatus.Failure;
        c.blackboard.Set("slamReady", c.time + 5f);
        Debug.Log("Slam!");
        return BTStatus.Success;
    }
}
}
