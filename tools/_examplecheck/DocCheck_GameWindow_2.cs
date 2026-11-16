namespace DocCheck_GameWindow
{
using Elintria;

public class PhotoKey : ElintriaScript
{
    bool wasDown;

    public override void Update()
    {
        bool down = GameWindow.KeyDown(0x2C);   // VK_SNAPSHOT (Print Screen)
        if (down && !wasDown)
        {
            GameWindow.Capture(Project.Resolve("capture_" + System.DateTime.Now.ToString("HHmmss") + ".bmp"));
            Debug.Log("Saved a " + GameWindow.width + "x" + GameWindow.height + " screenshot");
        }
        wasDown = down;
        if (GameWindow.MousePosition(out float mx, out float my)) ScreenGUI.Text(10, 10, "Pointer " + mx + ", " + my);
    }
}
}
