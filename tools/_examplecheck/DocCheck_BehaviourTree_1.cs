namespace DocCheck_BehaviourTree
{
using System.Collections.Generic;
using Elintria;
using Elintria.AI;

public class Guard : ElintriaScript
{
    public GameObject player;
    public List<GameObject> waypoints = new List<GameObject>();
    public float sightRange = 12f;
    int next;

    public override void Update() { this.RunBehaviourTree("Assets/AI/Guard.elbt"); }

    [BTCondition] bool SeesPlayer() =>
        player != null && Vector3.Distance(player.transform.position, transform.position) < sightRange;

    [BTAction] BTStatus Chase()
    {
        NavAgent.Of(gameObject).SetDestination(player.transform.position);
        return SeesPlayer() ? BTStatus.Running : BTStatus.Success;
    }

    [BTAction] BTStatus GoToNextWaypoint()
    {
        if (waypoints.Count == 0) return BTStatus.Failure;
        NavAgent a = NavAgent.Of(gameObject);
        Vector3 goal = waypoints[next].transform.position;
        if (a.destination != goal) a.SetDestination(goal);
        if (a.remainingDistance > 0.5f) return BTStatus.Running;
        next = (next + 1) % waypoints.Count;
        return BTStatus.Success;
    }
}
}
