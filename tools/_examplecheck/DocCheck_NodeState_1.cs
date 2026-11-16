namespace DocCheck_NodeState
{
using Elintria;
using Elintria.AI;

public sealed class HoldNode : BTNode
{
    protected override void Enter(BTContext c, BTInstance.NodeState s) { s.until = c.time + Num("seconds", 1f); }
    protected override BTStatus Run(BTContext c, BTInstance.NodeState s) => c.time < s.until ? BTStatus.Running : BTStatus.Success;
}
}
