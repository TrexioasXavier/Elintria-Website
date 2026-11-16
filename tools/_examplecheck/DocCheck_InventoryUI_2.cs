namespace DocCheck_InventoryUI
{
using Elintria;
using Elintria.Standard;

public class SlotTooltips : ElintriaScript
{
    public override void Update()
    {
        InventoryUI ui = GetComponent<InventoryUI>();
        Inventory inv = GetComponent<Inventory>();
        if (!ui.open) return;
        for (int i = 0; i < inv.slots.Length; i++)
        {
            Rect r = ui.SlotRect(i);
            if (inv.slots[i].IsEmpty || !ScreenGUI.Hover(r)) continue;
            ItemDefinition def = ItemDatabase.Get(inv.slots[i].id);
            ScreenGUI.Label(new Rect(r.x, r.y - 26, 260, 24), def.displayName + ": " + def.description, null, Color.white, TextAnchor.MiddleLeft, false, Color.black);
        }
    }
}
}
