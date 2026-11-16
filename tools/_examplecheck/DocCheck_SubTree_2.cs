namespace DocCheck_SubTree
{
using Elintria;
using Elintria.AI;

public class ReloadTrees : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            BehaviourTree.Reload();   // sub-trees are read again too
            Debug.Log("Behaviour trees reloaded");
        }
    }
}
}
