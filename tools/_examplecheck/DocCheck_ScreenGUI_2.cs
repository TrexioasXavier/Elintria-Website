namespace DocCheck_ScreenGUI
{
using Elintria;

public class QuestLog : ElintriaScript
{
    string[] quests = { "Find the key", "Open the vault", "Talk to Mira", "Repair the pump", "Escape the lab", "Feed the cat" };
    float scroll;

    public override void Update()
    {
        Rect panel = new Rect(40, 100, 300, 160);
        ScreenGUI.Fill(panel, new Color(0f, 0f, 0f, 0.6f));
        if (ScreenGUI.Hover(panel)) scroll = Mathf.Clamp(scroll - ScreenGUI.Wheel() * 20f, 0f, Mathf.Max(0f, quests.Length * 32 - panel.height));
        ScreenGUI.PushClip(panel);
        for (int i = 0; i < quests.Length; i++)
        {
            Rect row = new Rect(panel.x + 8, panel.y + 4 + i * 32 - scroll, panel.width - 16, 28);
            if (ScreenGUI.Hover(row)) ScreenGUI.Fill(row, new Color(1f, 1f, 1f, 0.1f));
            ScreenGUI.Label(row, quests[i], null, Color.white, TextAnchor.MiddleLeft);
        }
        ScreenGUI.PopClip();
    }
}
}
