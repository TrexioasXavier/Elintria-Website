namespace DocCheck_NavAgent
{
using System.Collections.Generic;
using Elintria;

public class GuardPatrol : ElintriaScript
{
    public List<GameObject> points = new List<GameObject>();
    int next;
    NavAgent agent;

    public override void Start()
    {
        agent = NavAgent.Of(gameObject);
        agent.speed = 2f;
        if (points.Count > 0) agent.SetDestination(points[0].transform.position);
    }

    public override void Update()
    {
        if (!agent.exists || points.Count == 0) return;
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            next = (next + 1) % points.Count;
            agent.SetDestination(points[next].transform.position);
        }
    }
}
}
