namespace DocCheck_QuestGiver
{
using Elintria;
using Elintria.Standard;

public class QuestTrigger : ElintriaScript
{
    public QuestGiver giver;

    public void OnTriggered() { if (!giver.Offered) giver.Offer(); }
}
}
