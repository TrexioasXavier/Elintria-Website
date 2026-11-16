namespace DocCheck_BehaviourTreeExtensions
{
using Elintria;
using Elintria.AI;

public class Messenger : ElintriaScript
{
    const string Tree = "Assets/AI/Messenger.elbt";

    public override void Update() { this.RunBehaviourTree(Tree); }

    public void Deliver(GameObject to)
    {
        BTInstance bt = this.BehaviourTreeAgent(Tree);
        if (bt != null) bt.blackboard.Set("target", to);
    }
}
}
