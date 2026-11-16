namespace DocCheck_QuestLog
{
using Elintria;
using Elintria.Standard;

public class QuestCount : ElintriaScript
{
    public override void Update()
    {
        int done = 0;
        foreach (Quest q in QuestLog.quests) if (q.done) ++done;
        ScreenGUI.Text(20f, 60f, "Quests done: " + done);
    }
}
}
