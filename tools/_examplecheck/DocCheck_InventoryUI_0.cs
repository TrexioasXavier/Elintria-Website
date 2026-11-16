namespace DocCheck_InventoryUI
{
using Elintria;
using Elintria.Standard;

public class BagButton : ElintriaScript
{
    public override void Update()
    {
        InventoryUI ui = GetComponent<InventoryUI>();
        if (ScreenGUI.Button(new Rect(ScreenGUI.screenWidth - 110, 10, 100, 36), ui.open ? "Close" : "Bag")) ui.Toggle();
    }
}
}
