namespace DocCheck_FixedJoint
{
using Elintria;

public class GlueGrab : ElintriaScript
{
    public GameObject hand;
    FixedJoint held;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && held == null && hand != null)
        {
            held = gameObject.AddComponent<FixedJoint>();
            held.connectedBody = hand.GetComponent<Rigidbody>();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && held != null)
        {
            held.Break();
            held = null;
        }
    }
}
}
