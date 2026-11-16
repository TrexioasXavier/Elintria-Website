namespace DocCheck_QuestLog
{
using Elintria;
using Elintria.Standard;

public class StepDoor : ElintriaScript
{
    public string questId = "power";

    public override void Start()
    {
        QuestLog.StepDone += (q, s) => { if (q.id == questId && s.kind == "goto") GetComponent<Door>().Open(); };
    }
}
}
