namespace DocCheck_BTStatus
{
using Elintria;
using Elintria.AI;

public class OpenChest : ElintriaScript
{
    public GameObject chest;
    float progress;

    public override void Update() { this.RunBehaviourTree("Assets/AI/Looter.elbt"); }

    BTStatus Open(BTContext c)
    {
        if (chest == null) return BTStatus.Failure;
        progress += c.deltaTime;
        if (progress < 1.5f) return BTStatus.Running;
        progress = 0f;
        return BTStatus.Success;
    }
}
}
