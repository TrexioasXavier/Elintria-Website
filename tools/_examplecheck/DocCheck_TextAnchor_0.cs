namespace DocCheck_TextAnchor
{
using Elintria;

public class CenterTitle : ElintriaScript
{
    public override void Update()
    {
        ScreenGUI.Label(new Rect(0, 0, ScreenGUI.screenWidth, ScreenGUI.screenHeight), "LEVEL CLEAR", null, Color.yellow, TextAnchor.MiddleCenter);
    }
}
}
