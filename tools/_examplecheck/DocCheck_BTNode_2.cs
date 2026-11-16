namespace DocCheck_BTNode
{
using Elintria;
using Elintria.AI;

public class TreeInspector : ElintriaScript
{
    public string treePath = "Assets/AI/Guard.elbt";

    public override void Start() { Print(BehaviourTree.Load(treePath).root, 0); }

    void Print(BTNode n, int depth)
    {
        string extra = n.type == "Action" ? " -> " + n.Str("action") : n.type == "Wait" ? " " + n.Num("seconds", 1f) + "s" : "";
        Debug.Log(new string(' ', depth * 2) + "#" + n.id + " " + n.type + extra);
        foreach (BTNode child in n.children) Print(child, depth + 1);
    }
}
}
