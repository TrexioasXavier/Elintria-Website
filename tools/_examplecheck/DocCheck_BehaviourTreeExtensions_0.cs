namespace DocCheck_BehaviourTreeExtensions
{
using Elintria;
using Elintria.AI;

public class Chicken : ElintriaScript
{
    public override void Update() { this.RunBehaviourTree("Assets/AI/Chicken.elbt"); }

    BTStatus Peck() { transform.Rotate(10f * Time.deltaTime, 0f, 0f); return BTStatus.Success; }
    bool Scared() => Physics.CheckSphere(transform.position, 3f);
}
}
