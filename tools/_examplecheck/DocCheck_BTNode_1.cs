namespace DocCheck_BTNode
{
using Elintria;
using Elintria.AI;

public class CodeTree : ElintriaScript
{
    BTInstance bt;

    public override void Start()
    {
        var chance = new ChanceNode();
        chance.props["chance"] = 0.25f;
        var sneeze = BehaviourTree.Parse(@"{ ""root"": { ""type"": ""Action"", ""props"": { ""action"": ""Sneeze"" } } }").root;

        var seq = new Sequence();
        seq.children.Add(chance);
        seq.children.Add(sneeze);

        bt = new BehaviourTree { root = seq }.Instance(this, gameObject);
    }

    public override void Update() { bt.Tick(); }

    void Sneeze() { Debug.Log("Achoo!"); }
}
}
