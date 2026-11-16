namespace DocCheck_BTInstance
{
using Elintria;
using Elintria.AI;

public class BrainDebug : ElintriaScript
{
    public override void Update()
    {
        BehaviourTreeRunner r = GetComponent<BehaviourTreeRunner>();
        BTInstance bt = r != null ? r.Agent : null;
        if (bt == null) return;
        ScreenGUI.Text(20, 20, name + ": tick " + bt.tickCount + ", last " + bt.lastResult);
        foreach (var kv in bt.blackboard.All) ScreenGUI.Text(20, 40, kv.Key + " = " + kv.Value);
    }
}
}
