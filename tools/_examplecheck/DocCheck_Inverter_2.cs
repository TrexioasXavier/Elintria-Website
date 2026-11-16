namespace DocCheck_Inverter
{
using Elintria;
using Elintria.AI;

public class Thief : ElintriaScript
{
    public GameObject guard;

    public override void Update() { this.RunBehaviourTree("Assets/AI/Thief.elbt"); }

    bool GuardSeesMe() => guard != null && Vector3.Angle(guard.transform.forward, transform.position - guard.transform.position) < 40f;
    void StealItem() { Debug.Log("Got it!"); }
}
}
