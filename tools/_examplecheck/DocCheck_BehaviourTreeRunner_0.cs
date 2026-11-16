namespace DocCheck_BehaviourTreeRunner
{
using Elintria;
using Elintria.AI;

public class Zombie : ElintriaScript
{
    public float smellRange = 20f;
    GameObject player;

    public override void Start() { player = GameObject.FindWithTag("Player"); }

    bool SmellsPlayer() => player != null && Vector3.Distance(player.transform.position, transform.position) < smellRange;
    bool InReach() => player != null && Vector3.Distance(player.transform.position, transform.position) < 1.5f;

    BTStatus Shamble()
    {
        NavAgent.Of(gameObject).SetDestination(player.transform.position);
        return InReach() ? BTStatus.Success : BTStatus.Running;
    }

    void Bite() { player.SendMessage("ApplyDamage", 15f); }
}
}
