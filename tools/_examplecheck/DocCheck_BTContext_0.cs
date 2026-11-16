namespace DocCheck_BTContext
{
using Elintria;
using Elintria.AI;

public class Channel : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Mage.elbt"); }

    BTStatus ChannelSpell(BTContext c)
    {
        float t = c.blackboard.Float("channel") + c.deltaTime;
        c.blackboard.Set("channel", t);
        if (t < 2f) return BTStatus.Running;
        c.blackboard.Set("channel", 0f);
        Debug.Log("Fireball!");
        return BTStatus.Success;
    }
}
}
