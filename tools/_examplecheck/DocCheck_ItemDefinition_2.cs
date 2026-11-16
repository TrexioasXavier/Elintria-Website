namespace DocCheck_ItemDefinition
{
using System.Linq;
using Elintria;
using Elintria.Standard;

public class BagSorter : ElintriaScript
{
    public void Sort()
    {
        Inventory inv = GetComponent<Inventory>();
        var stacks = inv.slots.Where(s => !s.IsEmpty).OrderBy(s => ItemDatabase.Get(s.id)?.category ?? "").ThenBy(s => s.id).ToList();
        inv.Clear();
        foreach (ItemStack s in stacks) inv.Add(s.id, s.count);
    }
}
}
