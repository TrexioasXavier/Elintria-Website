namespace DocCheck_BehaviourTreeExtensions
{
using Elintria;
using Elintria.AI;

public class MoodSwitch : ElintriaScript
{
    public bool angry;

    public override void Update()
    {
        this.RunBehaviourTree(angry ? "Assets/AI/Hunt.elbt" : "Assets/AI/Graze.elbt");
    }
}
}
