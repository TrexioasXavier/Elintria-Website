namespace DocCheck_Blackboard
{
using Elintria;
using Elintria.AI;

public class Needs : ElintriaScript
{
    const string Tree = "Assets/AI/Sim.elbt";

    public override void Update()
    {
        this.RunBehaviourTree(Tree);
        Blackboard b = this.BehaviourTreeAgent(Tree).blackboard;
        b.Set("hunger", Mathf.Clamp01(b.Float("hunger") + 0.02f * Time.deltaTime));
        b.Set("tired", Mathf.Clamp01(b.Float("tired") + 0.01f * Time.deltaTime));
    }

    void Eat(BTContext c) { c.blackboard.Set("hunger", 0f); }
    void Sleep(BTContext c) { c.blackboard.Set("tired", 0f); }
}
}
