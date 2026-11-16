namespace DocCheck_Failer
{
using Elintria;
using Elintria.AI;

public class ForceFallback : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Debugged.elbt"); }
    void DoRealWork() { Debug.Log("Reached the real work, after the Failer branch"); }
}
}
