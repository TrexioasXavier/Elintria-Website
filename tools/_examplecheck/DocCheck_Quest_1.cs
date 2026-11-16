namespace DocCheck_Quest
{
using Elintria;
using Elintria.Standard;

public class SkipStep : ElintriaScript
{
    public override void Update()
    {
        Quest q = QuestLog.Active;
        if (q != null && Input.GetKeyDown(KeyCode.F8)) ++q.current;
    }
}
}
