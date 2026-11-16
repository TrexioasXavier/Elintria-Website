namespace DocCheck_Quest
{
using Elintria;
using Elintria.Standard;

public class CurrentStep : ElintriaScript
{
    public override void Update()
    {
        Quest q = QuestLog.Active;
        if (q != null && q.Step != null) ScreenGUI.Text(20f, 80f, q.title + ": " + q.Step.text);
    }
}
}
