namespace DocCheck_CustomYieldInstruction
{
using System.Collections;
using Elintria;

public class WaitForArrival : CustomYieldInstruction
{
    readonly NavAgent agent;
    public WaitForArrival(NavAgent a) { agent = a; }
    public override bool keepWaiting => agent.exists && (agent.pathStatus != 2 || agent.remainingDistance > 0.3f);
}

public class Patrol : ElintriaScript
{
    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        NavAgent agent = NavAgent.Of(gameObject);
        agent.SetDestination(new Vector3(10f, 0f, 10f));
        yield return new WaitForArrival(agent);
        Debug.Log("Arrived");
    }
}
}
