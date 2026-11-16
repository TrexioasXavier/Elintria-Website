namespace DocCheck_GuiFont
{
using Elintria;

public class FitTitle : ElintriaScript
{
    public string title = "The Long Descent";

    public override void Update()
    {
        int size = 48;
        while (size > 12 && GuiFont.Get("", size, true).Measure(title) > 300f) size -= 2;
        ScreenGUI.Text(40, 40, title, GuiFont.Get("", size, true));
    }
}
}
