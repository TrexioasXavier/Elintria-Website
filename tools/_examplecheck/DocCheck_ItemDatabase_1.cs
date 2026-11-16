namespace DocCheck_ItemDatabase
{
using Elintria;
using Elintria.Standard;

public class GiveAll : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F9)) return;
        Inventory inv = GetComponent<Inventory>();
        foreach (ItemDefinition d in ItemDatabase.All) inv.Add(d.id, 1);
    }
}
}
