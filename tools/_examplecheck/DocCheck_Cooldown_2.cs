namespace DocCheck_Cooldown
{
using Elintria;
using Elintria.AI;

public class Wizard : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Wizard.elbt"); }
    void CastFireball() { Debug.Log("FIREBALL"); }
    void MagicMissile() { Debug.Log("missile"); }
}
}
