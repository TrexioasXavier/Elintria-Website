namespace DocCheck_TextAnchor
{
using Elintria;

public class CornerLabels : ElintriaScript
{
    public override void Update()
    {
        Rect all = new Rect(10, 10, ScreenGUI.screenWidth - 20, ScreenGUI.screenHeight - 20);
        ScreenGUI.Label(all, "Score 1200", null, Color.white, TextAnchor.UpperLeft);
        ScreenGUI.Label(all, "Lives 3", null, Color.white, TextAnchor.UpperRight);
        ScreenGUI.Label(all, "v1.0", null, Color.gray, TextAnchor.LowerRight);
    }
}
}
