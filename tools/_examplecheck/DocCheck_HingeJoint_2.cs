namespace DocCheck_HingeJoint
{
using Elintria;

public class SpringGate : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.up;
        h.useSpring = true;
        h.spring = new JointSpring { spring = 40f, damper = 6f, targetPosition = 0f };
    }

    public override void Update()
    {
        if (Mathf.Abs(GetComponent<HingeJoint>().angle) > 60f) Debug.Log("Gate wide open");
    }
}
}
