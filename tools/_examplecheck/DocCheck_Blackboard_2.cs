namespace DocCheck_Blackboard
{
using Elintria;
using Elintria.AI;

public class BlackboardDump : ElintriaScript
{
    public override void Update()
    {
        BTInstance bt = this.BehaviourTreeAgent("Assets/AI/Sim.elbt");
        if (bt == null || !Input.GetKeyDown(KeyCode.B)) return;
        foreach (var kv in bt.blackboard.All) Debug.Log(kv.Key + " = " + kv.Value);
    }
}
}
