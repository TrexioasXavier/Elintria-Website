namespace DocCheck_Quest
{
using Elintria;
using Elintria.Standard;

public class QuestProgress : ElintriaScript
{
    public float Share(string id)
    {
        Quest q = QuestLog.Find(id);
        if (q == null || q.steps.Count == 0) return 0f;
        return q.done ? 1f : q.current / (float)q.steps.Count;
    }
}
}
