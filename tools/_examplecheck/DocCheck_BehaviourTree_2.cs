namespace DocCheck_BehaviourTree
{
using Elintria;
using Elintria.AI;

public class ManualTree : ElintriaScript
{
    public string treePath = "Assets/AI/Worker.elbt";
    BTInstance brain;

    public override void Start()
    {
        BehaviourTree tree = BehaviourTree.Load(treePath);
        if (tree == null) { Debug.LogError("No tree at " + treePath); return; }
        brain = tree.Instance(this, gameObject);
        brain.blackboard.Set("energy", 1f);
    }

    public override void Update()
    {
        if (brain == null) return;
        BTStatus r = brain.Tick();
        if (r == BTStatus.Failure) Debug.Log(name + ": nothing to do");
    }

    void Work() { brain.blackboard.Set("energy", brain.blackboard.Float("energy") - 0.01f); }
}
}
