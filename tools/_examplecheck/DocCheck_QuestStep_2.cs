namespace DocCheck_QuestStep
{
using Elintria;
using Elintria.Standard;

public class StepsLeft : ElintriaScript
{
    public override void Start()
    {
        QuestLog.Started += q => { foreach (QuestStep s in q.steps) Debug.Log(q.title + ": " + s.text); };
    }
}
}
