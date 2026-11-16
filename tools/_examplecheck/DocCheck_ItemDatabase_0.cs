namespace DocCheck_ItemDatabase
{
using Elintria;
using Elintria.Standard;

public class ItemList : ElintriaScript
{
    public override void Start()
    {
        foreach (ItemDefinition d in ItemDatabase.All) Debug.Log(d.id + ": " + d.displayName + " (stacks to " + d.maxStack + ")");
    }
}
}
