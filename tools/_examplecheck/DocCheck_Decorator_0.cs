namespace DocCheck_Decorator
{
using Elintria;
using Elintria.AI;

public sealed class ChanceGate : Decorator
{
    protected override BTStatus Run(BTContext c, BTInstance.NodeState s)
    {
        if (!s.open && Random.value > Num("chance", 0.5f)) return BTStatus.Failure;
        return children.Count > 0 ? children[0].Tick(c) : BTStatus.Failure;
    }
}
}
