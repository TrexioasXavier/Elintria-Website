namespace DocCheck_SpringJoint
{
using Elintria;

public class BouncyPlatform : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<Rigidbody>().mass = 10f;
        SpringJoint s = gameObject.AddComponent<SpringJoint>();
        s.anchor = new Vector3(0f, -1f, 0f);
        s.spring = 400f;
        s.damper = 10f;
        s.restLength = 1f;
    }
}
}
