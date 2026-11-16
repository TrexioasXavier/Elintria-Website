namespace DocCheck_UntilFail
{
using Elintria;
using Elintria.AI;

public class Glutton : ElintriaScript
{
    public int food = 5;
    public override void Update() { this.RunBehaviourTree("Assets/AI/Glutton.elbt"); }
    bool EatOneBite() { if (food <= 0) return false; food--; Debug.Log("Munch"); return true; }
}
}
