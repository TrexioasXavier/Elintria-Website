namespace DocCheck_SliderJoint
{
using Elintria;

public class Piston : ElintriaScript
{
    float t;

    public override void Start()
    {
        SliderJoint s = gameObject.AddComponent<SliderJoint>();
        s.axis = Vector3.up;
        s.minDistance = 0f;
        s.maxDistance = 2f;
        s.useMotor = true;
        s.motorForce = 500f;
    }

    public override void Update()
    {
        t += Time.deltaTime;
        GetComponent<SliderJoint>().motorSpeed = Mathf.Sin(t * 2f) > 0f ? 1.5f : -1.5f;
    }
}
}
