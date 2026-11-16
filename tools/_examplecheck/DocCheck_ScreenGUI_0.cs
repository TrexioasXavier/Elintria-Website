namespace DocCheck_ScreenGUI
{
using Elintria;

public class Hud : ElintriaScript
{
    public float health = 80f;
    public int ammo = 24;
    GuiFont big;

    public override void Start() { big = GuiFont.Get("", 28, true); }

    public override void Update()
    {
        int w = ScreenGUI.screenWidth, h = ScreenGUI.screenHeight;
        ScreenGUI.Bar(new Rect(20, h - 40, 240, 20), health / 100f, new Color(0.85f, 0.2f, 0.2f), new Color(0f, 0f, 0f, 0.5f));
        ScreenGUI.Border(new Rect(20, h - 40, 240, 20), Color.white);
        ScreenGUI.Label(new Rect(w - 200, h - 60, 180, 40), ammo + " / 120", big, Color.white, TextAnchor.MiddleRight, false, Color.black);
        ScreenGUI.Line(new Vector2(w / 2 - 8, h / 2), new Vector2(w / 2 + 8, h / 2), Color.white, 2f);
        ScreenGUI.Line(new Vector2(w / 2, h / 2 - 8), new Vector2(w / 2, h / 2 + 8), Color.white, 2f);
    }
}
}
