namespace DocCheck_ItemStack
{
using Elintria;
using Elintria.Standard;

public class Trade : ElintriaScript
{
    public static void GiveSlot(Inventory from, int slot, Inventory to)
    {
        ItemStack s = from.TakeSlot(slot);
        if (s.IsEmpty) return;
        int left = to.Add(s.id, s.count);
        if (left > 0) from.Add(s.id, left);   // give back what didn't fit
    }
}
}
