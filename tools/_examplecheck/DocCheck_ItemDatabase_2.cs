namespace DocCheck_ItemDatabase
{
using Elintria;
using Elintria.Standard;

public class ItemReload : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) { ItemDatabase.Reload(); Debug.Log("Item files reloaded"); }
    }
}
}
