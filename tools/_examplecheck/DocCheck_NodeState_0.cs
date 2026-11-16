namespace DocCheck_NodeState
{
using Elintria;
using Elintria.AI;

public sealed class EveryNthNode : BTNode
{
    protected override BTStatus Run(BTContext c, BTInstance.NodeState s)
    {
        s.count++;
        return s.count % (int)Num("n", 3) == 0 ? BTStatus.Success : BTStatus.Failure;
    }
}
}
