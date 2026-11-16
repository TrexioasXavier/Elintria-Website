namespace DocCheck_ConditionNode
{
using Elintria;
using Elintria.AI;

public class Healer : ElintriaScript
{
    public int potions = 2;
    public float health = 40f;
    public bool FullHealth => health >= 100f;   // a property works

    public override void Update() { this.RunBehaviourTree("Assets/AI/Healer.elbt"); }

    bool HasPotion() => potions > 0;
    void DrinkPotion() { potions--; health = Mathf.Min(100f, health + 50f); }
}
}
