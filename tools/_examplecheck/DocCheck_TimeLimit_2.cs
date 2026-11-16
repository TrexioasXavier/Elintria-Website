namespace DocCheck_TimeLimit
{
using Elintria;
using Elintria.AI;

public class Chaser : ElintriaScript
{
    public GameObject target;
    public override void Update() { this.RunBehaviourTree("Assets/AI/Chaser.elbt"); }

    BTStatus Chase() { NavAgent.Of(gameObject).SetDestination(target.transform.position); return BTStatus.Running; }
    void ChaseAbort() { NavAgent.Of(gameObject).ResetPath(); }   // called when the time limit cuts it short
    void ReturnHome() { Debug.Log("Lost them. Going home."); }
}
}
