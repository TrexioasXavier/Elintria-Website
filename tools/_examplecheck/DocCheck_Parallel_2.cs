namespace DocCheck_Parallel
{
using Elintria;
using Elintria.AI;

public class TalkWalk : ElintriaScript
{
    float said;

    public override void Update() { this.RunBehaviourTree("Assets/AI/TalkWalk.elbt"); }   // Parallel of WalkHome + Chatter

    BTStatus WalkHome() { transform.position += transform.forward * Time.deltaTime; return BTStatus.Running; }
    BTStatus Chatter(BTContext c)
    {
        if (c.time - said > 3f) { said = c.time; Debug.Log(name + ": lovely weather"); }
        return BTStatus.Running;
    }
}
}
