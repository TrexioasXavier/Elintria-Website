namespace DocCheck_SetValue
{
using Elintria;
using Elintria.AI;

public class Moods : ElintriaScript
{
    public bool insulted;
    const string Tree = "Assets/AI/Moods.elbt";

    public override void Update()
    {
        this.RunBehaviourTree(Tree);
        string mood = this.BehaviourTreeAgent(Tree).blackboard.Get<string>("mood", "calm");
        ScreenGUI.Text(20, 20, name + " is " + mood);
    }

    bool Insulted() => insulted;
}
}
