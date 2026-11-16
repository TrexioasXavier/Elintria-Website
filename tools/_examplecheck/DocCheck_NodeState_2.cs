namespace DocCheck_NodeState
{
using Elintria;
using Elintria.AI;

public class RootState : ElintriaScript
{
    public override void Update()
    {
        BTInstance bt = this.BehaviourTreeAgent("Assets/AI/Guard.elbt");
        if (bt == null) return;
        BTInstance.NodeState s = bt.State(bt.tree.root.id);
        ScreenGUI.Text(20, 60, "Root running: " + s.open + ", last " + s.last);
    }
}
}
