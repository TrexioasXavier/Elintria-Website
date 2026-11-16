namespace DocCheck_Wait
{
using Elintria;
using Elintria.AI;

public class Sentry2 : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Sentry2.elbt"); }
    void LookLeft() { transform.rotation = Quaternion.Euler(0f, -45f, 0f); }
    void LookRight() { transform.rotation = Quaternion.Euler(0f, 45f, 0f); }
}
}
