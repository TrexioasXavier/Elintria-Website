namespace DocCheck_QuestStep
{
using Elintria;
using Elintria.Standard;

public class StepBars : ElintriaScript
{
    public override void Update()
    {
        Quest q = QuestLog.Active;
        if (q == null) return;
        float y = 260f;
        foreach (QuestStep s in q.steps)
        {
            ScreenGUI.Bar(new Rect(20f, y, 150f, 6f), s.done ? 1f : s.progress / (float)Mathf.Max(1, s.count), Color.green, Color.black);
            y += 10f;
        }
    }
}
}
