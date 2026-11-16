namespace DocCheck_Succeeder
{
using Elintria;
using Elintria.AI;

public class Scavenger : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Scavenger.elbt"); }

    bool TakeAmmo() => Random.value < 0.5f;   // fails half the time; the Succeeder hides that
    void GoToCorpse() { }
    void MoveOn() { Debug.Log("Moving on"); }
}
}
