namespace DocCheck_Joint
{
using Elintria;

public class BreakableSign : ElintriaScript
{
    public override void Start()
    {
        FixedJoint j = gameObject.AddComponent<FixedJoint>();
        j.breakForce = 300f;
    }

    public override void Update()
    {
        FixedJoint j = GetComponent<FixedJoint>();
        if (j != null && j.broken) Debug.Log("The sign came off");
    }
}
}
