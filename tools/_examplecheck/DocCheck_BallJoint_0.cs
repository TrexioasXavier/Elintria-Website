namespace DocCheck_BallJoint
{
using Elintria;

public class HangingLamp : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<Rigidbody>().mass = 2f;
        BallJoint b = gameObject.AddComponent<BallJoint>();
        b.anchor = new Vector3(0f, 1.5f, 0f);     // the ceiling hook, above it
    }
}
}
