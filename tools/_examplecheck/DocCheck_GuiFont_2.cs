namespace DocCheck_GuiFont
{
using System.Collections.Generic;
using Elintria;

public class Typewriter : ElintriaScript
{
    public string text = "Day 14. The generator failed again. I can hear something in the vents.";
    GuiFont font;

    public override void Start() { font = GuiFont.Get("Consolas", 16); }

    public override void Update()
    {
        int shown = Mathf.Min(text.Length, (int)(Time.time * 25f));
        List<string> lines = font.Wrap(text.Substring(0, shown), 320f);
        for (int i = 0; i < lines.Count; i++)
            ScreenGUI.Text(40, 300 + i * font.lineHeight, lines[i], font, Color.green);
    }
}
}
