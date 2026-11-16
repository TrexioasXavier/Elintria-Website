namespace DocCheck_BTInstance
{
using Elintria;
using Elintria.AI;

public class StunReset : ElintriaScript
{
    public void OnStunned()
    {
        BTInstance bt = this.BehaviourTreeAgent("Assets/AI/Soldier.elbt");
        if (bt == null) return;
        bt.Reset();
        bt.blackboard.Set("stunnedUntil", Time.time + 2f);
    }
}
}
