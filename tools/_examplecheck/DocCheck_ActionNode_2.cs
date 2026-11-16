namespace DocCheck_ActionNode
{
using Elintria;
using Elintria.AI;

public class MissingAction : ElintriaScript
{
    public override void Update()
    {
        // The tree names "Fly", which this script lacks: that node simply fails.
        this.RunBehaviourTree("Assets/AI/Bird.elbt");
    }
}
}
