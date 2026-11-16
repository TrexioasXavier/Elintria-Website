namespace DocCheck_GuiFont
{
using Elintria;

public class SpeechBubble : ElintriaScript
{
    public string line = "You shouldn't be down here. The tunnels flood at night.";
    GuiFont font;

    public override void Start() { font = GuiFont.Get("", 18); }

    public override void Update()
    {
        Vector2 size = font.MeasureWrapped(line, 260f);
        Rect box = new Rect(60, 60, size.x + 20, size.y + 16);
        ScreenGUI.Fill(box, new Color(1f, 1f, 1f, 0.9f));
        ScreenGUI.Label(new Rect(box.x + 10, box.y + 8, size.x, size.y), line, font, Color.black, TextAnchor.UpperLeft, true);
    }
}
}
