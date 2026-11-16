namespace DocCheck_BTInstance
{
using Elintria;
using Elintria.AI;

public class SlowThinker : ElintriaScript
{
    public float thinksPerSecond = 4f;
    BTInstance bt;
    float next;

    public override void Start() { bt = BehaviourTree.Load("Assets/AI/Villager.elbt").Instance(this, gameObject); }

    public override void Update()
    {
        if (Time.time < next) return;
        next = Time.time + 1f / thinksPerSecond;
        bt.Tick();
    }
}
}
