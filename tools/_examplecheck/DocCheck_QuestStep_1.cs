namespace DocCheck_QuestStep
{
using Elintria;
using Elintria.Standard;

public class CountBosses : ElintriaScript
{
    public override void Start()
    {
        StdEvents.Died += (h, killer) =>
        {
            Quest q = QuestLog.Active;
            if (q != null && q.Step != null && q.Step.kind == "kill" && h.maxHealth >= 500f) ++q.Step.progress;
        };
    }
}
}
