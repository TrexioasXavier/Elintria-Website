namespace DocCheck_JointLimits
{
using Elintria;

public class LeverStops : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.right;
        h.useLimits = true;
        h.limits = new JointLimits(-45f, 45f);
    }

    public override void Update()
    {
        HingeJoint h = GetComponent<HingeJoint>();
        if (h.angle > h.limits.max - 2f) Debug.Log("Lever up");
    }
}
}
