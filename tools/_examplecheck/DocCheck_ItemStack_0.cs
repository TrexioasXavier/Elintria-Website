namespace DocCheck_ItemStack
{
using Elintria;
using Elintria.Standard;

public class FullSlots : ElintriaScript
{
    public override void Update()
    {
        int used = 0;
        foreach (ItemStack s in GetComponent<Inventory>().slots) if (!s.IsEmpty) used++;
        ScreenGUI.Text(20, 20, "Bag: " + used + " slots used");
    }
}
}
