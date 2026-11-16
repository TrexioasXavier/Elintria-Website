namespace DocCheck_BTContext
{
using Elintria;
using Elintria.AI;

public class OwnerAware : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Turret.elbt"); }

    void Report(BTContext c)
    {
        GameObject me = c.instance.owner;
        Debug.Log((me != null ? me.name : "?") + " ticked " + c.instance.tickCount + " times");
    }
}
}
