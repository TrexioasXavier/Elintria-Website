namespace DocCheck_ItemDefinition
{
using Elintria;
using Elintria.Standard;

public class ItemCard : ElintriaScript
{
    public string itemId = "potion";

    public override void Update()
    {
        ItemDefinition d = ItemDatabase.Get(itemId);
        if (d == null) return;
        ScreenGUI.Fill(new Rect(20, 20, 260, 70), new Color(0f, 0f, 0f, 0.6f));
        ScreenGUI.Text(30, 26, d.displayName + "  (" + d.category + ")", null, d.color);
        ScreenGUI.Text(30, 50, d.description + "  Stack " + d.maxStack);
    }
}
}
