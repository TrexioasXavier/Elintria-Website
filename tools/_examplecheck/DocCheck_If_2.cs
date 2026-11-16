namespace DocCheck_If
{
using Elintria;
using Elintria.AI;

public class Nightwatch : ElintriaScript
{
    const string Tree = "Assets/AI/Nightwatch.elbt";

    public override void Update()
    {
        this.RunBehaviourTree(Tree);
        this.BehaviourTreeAgent(Tree).blackboard.Set("isDay", (Time.time % 120f) < 60f);
    }

    BTStatus PatrolWithLantern() { transform.Rotate(0f, 30f * Time.deltaTime, 0f); return BTStatus.Running; }
}
}
