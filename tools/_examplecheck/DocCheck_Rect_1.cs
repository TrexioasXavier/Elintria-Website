namespace DocCheck_Rect
{
using Elintria;

public class CenterPanel : ElintriaScript
{
    public override void Update()
    {
        Rect panel = new Rect(0, 0, 360, 200);
        panel.position = new Vector2(ScreenGUI.screenWidth, ScreenGUI.screenHeight) * 0.5f - panel.size * 0.5f;
        ScreenGUI.Fill(panel, new Color(0f, 0f, 0f, 0.7f));
        ScreenGUI.Label(panel, "Game Over", null, Color.white, TextAnchor.MiddleCenter);
    }
}
}
