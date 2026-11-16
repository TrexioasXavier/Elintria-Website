namespace DocCheck_Recipe
{
using Elintria;
using Elintria.Standard;

public class MasterCrafter : ElintriaScript
{
    public bool Craft(Recipe r)
    {
        Inventory inv = GetComponent<Inventory>();
        if (!r.Make(inv)) return false;
        inv.Add(r.result, r.count);      // a second batch for free
        return true;
    }
}
}
