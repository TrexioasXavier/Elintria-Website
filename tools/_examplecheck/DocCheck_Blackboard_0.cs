namespace DocCheck_Blackboard
{
using Elintria;
using Elintria.AI;

public class Hunter : ElintriaScript
{
    const string Tree = "Assets/AI/Hunter.elbt";
    public GameObject player;

    public override void Update()
    {
        BTInstance bt = this.BehaviourTreeAgent(Tree);
        if (bt != null && CanSee()) bt.blackboard.Set("lastSeen", player.transform.position);
        this.RunBehaviourTree(Tree);
    }

    bool CanSee() => Physics.Linecast(transform.position, player.transform.position, out RaycastHit h) && h.gameObject == player;

    BTStatus Investigate(BTContext c)
    {
        if (!c.blackboard.Has("lastSeen")) return BTStatus.Failure;
        Vector3 spot = c.blackboard.Get<Vector3>("lastSeen");
        NavAgent.Of(gameObject).SetDestination(spot);
        return Vector3.Distance(transform.position, spot) > 1f ? BTStatus.Running : BTStatus.Success;
    }
}
}
