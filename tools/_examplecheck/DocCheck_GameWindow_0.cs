namespace DocCheck_GameWindow
{
using Elintria;

public class EscapeMenu : ElintriaScript
{
    bool open;

    public override void Start() { GameWindow.SetEscapeQuits(false); }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { open = !open; Time.timeScale = open ? 0f : 1f; }
        if (!open) return;
        if (ScreenGUI.Button(new Rect(GameWindow.width / 2 - 100, 200, 200, 48), "Resume")) { open = false; Time.timeScale = 1f; }
        if (ScreenGUI.Button(new Rect(GameWindow.width / 2 - 100, 260, 200, 48), "Quit")) GameWindow.Quit();
    }
}
}
