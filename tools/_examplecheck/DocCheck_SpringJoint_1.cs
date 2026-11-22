namespace DocCheck_SpringJoint
{
using Elintria;

public class Bungee : ElintriaScript
{
    public GameObject jumper;

    public override void Start()
    {
        if (jumper == null) return;
        SpringJoint s = jumper.AddComponent<SpringJoint>();
        s.connectedObject = gameObject;
        s.spring = 60f;
        s.damper = 2f;
        s.restLength = 8f;
        s.maxDistance = 14f;
    }
}
}
