namespace DocCheck_Sequence
{
using Elintria;
using Elintria.AI;

public class Medic : ElintriaScript
{
    GameObject hurtAlly;

    public override void Update() { this.RunBehaviourTree("Assets/AI/Medic.elbt"); }

    bool AllyHurt()
    {
        hurtAlly = GameObject.FindWithTag("Wounded");
        return hurtAlly != null;
    }

    BTStatus RunToAlly()
    {
        NavAgent.Of(gameObject).SetDestination(hurtAlly.transform.position);
        return Vector3.Distance(transform.position, hurtAlly.transform.position) > 1.5f ? BTStatus.Running : BTStatus.Success;
    }

    void Heal() { hurtAlly.SendMessage("Heal", 50f); hurtAlly.tag = "Untagged"; }
}
}
