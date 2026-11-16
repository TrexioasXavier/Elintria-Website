namespace DocCheck_RandomSelector
{
using Elintria;
using Elintria.AI;

public class BossAttacks : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Boss.elbt"); }   // RandomSelector of the three

    bool Stomp() { Debug.Log("Stomp"); return true; }
    bool Charge() { Debug.Log("Charge"); return true; }
    bool Roar() { Debug.Log("Roar"); return true; }
}
}
