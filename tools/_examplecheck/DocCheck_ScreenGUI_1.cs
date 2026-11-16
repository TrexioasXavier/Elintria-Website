namespace DocCheck_ScreenGUI
{
using Elintria;

public class TitleMenu : ElintriaScript
{
    public override void Update()
    {
        float cx = ScreenGUI.screenWidth / 2f;
        ScreenGUI.Label(new Rect(0, 120, ScreenGUI.screenWidth, 60), "MY GAME", GuiFont.Get("", 48, true), Color.white, TextAnchor.MiddleCenter);
        if (ScreenGUI.Button(new Rect(cx - 110, 240, 220, 50), "Play")) SceneManager.LoadScene("Level1");
        if (ScreenGUI.Button(new Rect(cx - 110, 300, 220, 50), "Options", null, null, null, false)) { }   // greyed out
        if (ScreenGUI.Button(new Rect(cx - 110, 360, 220, 50), "Quit")) GameWindow.Quit();
    }
}
}
