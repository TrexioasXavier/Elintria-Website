namespace DocCheck_Vector2
{
using Elintria;

public class ClickTarget : ElintriaScript
{
    Rect button = new Rect(40, 40, 160, 48);
    int clicks;

    public override void Update()
    {
        Vector2 m = Input.mousePosition;
        bool over = button.Contains(m);
        ScreenGUI.Fill(button, over ? Color.cyan : Color.gray);
        ScreenGUI.Label(button, "Clicks: " + clicks, null, Color.black, TextAnchor.MiddleCenter);
        if (over && Input.GetMouseButtonDown(0)) clicks++;
    }
}
}
