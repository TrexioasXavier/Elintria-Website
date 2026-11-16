namespace DocCheck_BTNode
{
using Elintria;
using Elintria.AI;

public sealed class ChanceNode : BTNode
{
    protected override BTStatus Run(BTContext c, BTInstance.NodeState s)
    {
        return Random.value < Num("chance", 0.5f) ? BTStatus.Success : BTStatus.Failure;
    }
}
}
