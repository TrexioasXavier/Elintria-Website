namespace DocCheck_Selector
{
using Elintria;
using Elintria.AI;

public class AbortAware : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Soldier.elbt"); }

    BTStatus Patrol() { NavAgent.Of(gameObject).isStopped = false; return BTStatus.Running; }
    void PatrolAbort() { NavAgent.Of(gameObject).ResetPath(); }   // called when Attack takes over
}
}
