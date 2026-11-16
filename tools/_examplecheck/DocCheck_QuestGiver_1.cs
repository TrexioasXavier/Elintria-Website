namespace DocCheck_QuestGiver
{
using Elintria;
using Elintria.Standard;

public class ChainedQuest : ElintriaScript
{
    public override void Start() { GetComponent<QuestGiver>().requires = "quest:wolves:done"; }
}
}
