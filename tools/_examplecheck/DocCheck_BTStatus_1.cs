namespace DocCheck_BTStatus
{
using Elintria;
using Elintria.AI;

public class SimpleActions : ElintriaScript
{
    public int ammo = 3;

    public override void Update() { this.RunBehaviourTree("Assets/AI/Shooter.elbt"); }

    bool Shoot() { if (ammo <= 0) return false; ammo--; return true; }   // Success / Failure
    void Reload() { ammo = 3; }                                           // always Success
}
}
