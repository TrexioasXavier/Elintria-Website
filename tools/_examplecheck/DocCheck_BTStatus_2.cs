namespace DocCheck_BTStatus
{
using Elintria;
using Elintria.AI;

public class ResultWatcher : ElintriaScript
{
    public override void Update()
    {
        BTStatus r = this.RunBehaviourTree("Assets/AI/Quest.elbt");
        if (r == BTStatus.Success) { Debug.Log("Quest AI finished"); enabled = false; }
        else if (r == BTStatus.Failure) Debug.LogWarning("Quest AI is stuck");
    }
}
}
