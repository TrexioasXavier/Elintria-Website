namespace DocCheck_BehaviourTree
{
using Elintria;
using Elintria.AI;

public class InlineTree : ElintriaScript
{
    BTInstance bt;

    public override void Start()
    {
        BehaviourTree t = BehaviourTree.Parse(@"{ ""root"": { ""type"": ""Sequence"", ""children"": [
            { ""type"": ""Action"", ""props"": { ""action"": ""Bark"" } },
            { ""type"": ""Wait"", ""props"": { ""seconds"": 3 } } ] } }");
        bt = t.Instance(this, gameObject);
    }

    public override void Update() { bt.Tick(); }

    void Bark() { Debug.Log(name + ": Woof!"); }
}
}
