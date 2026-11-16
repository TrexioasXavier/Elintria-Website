namespace DocCheck_BehaviourTreeRunner
{
using Elintria;
using Elintria.AI;

public class AddBrain : ElintriaScript
{
    public string treePath = "Assets/AI/Critter.elbt";

    public override void Awake()
    {
        BehaviourTreeRunner r = gameObject.AddComponent<BehaviourTreeRunner>();
        r.tree = treePath;   // set before its Start runs
    }

    void Wander() { transform.Rotate(0f, Random.Range(-30f, 30f), 0f); }
}
}
