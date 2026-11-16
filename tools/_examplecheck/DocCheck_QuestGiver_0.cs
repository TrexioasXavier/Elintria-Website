namespace DocCheck_QuestGiver
{
using Elintria;
using Elintria.Standard;

public class BountyBoard : ElintriaScript
{
    public override void Start()
    {
        QuestGiver g = gameObject.AddComponent<QuestGiver>();
        g.questId = "wolves";
        g.title = "Wolf Bounty";
        g.steps = "kill:Wolf:5:Hunt five wolves|talk:Hunter::Claim the bounty";
        g.reward = "money:80, xp:150";
    }
}
}
