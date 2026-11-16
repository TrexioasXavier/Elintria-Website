namespace DocCheck_ActionNode
{
using Elintria;
using Elintria.AI;

public class Miner : ElintriaScript
{
    public int ore;

    public override void Update() { this.RunBehaviourTree("Assets/AI/Miner.elbt"); }

    BTStatus Dig(BTContext c)                 // Running until enough ore
    {
        ore += Random.value < c.deltaTime ? 1 : 0;
        return ore >= 5 ? BTStatus.Success : BTStatus.Running;
    }

    bool Deposit() { if (ore == 0) return false; ore = 0; return true; }
    void DigAbort() { Debug.Log("Stopped digging"); }
}
}
