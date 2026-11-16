namespace DocCheck_Decorator
{
using Elintria;
using Elintria.AI;

public class ChanceGateUser : ElintriaScript
{
    BTInstance bt;

    public override void Start()
    {
        var gate = new ChanceGate();
        gate.props["chance"] = 0.3f;
        gate.children.Add(BehaviourTree.Parse(@"{ ""root"": { ""type"": ""Action"", ""props"": { ""action"": ""Taunt"" } } }").root);
        bt = new BehaviourTree { root = gate }.Instance(this, gameObject);
    }

    public override void Update() { bt.Tick(); }
    void Taunt() { Debug.Log("Come on then!"); }
}
}
