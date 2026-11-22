namespace DocCheck_SliderJoint
{
using Elintria;

public class Drawer : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        SliderJoint s = gameObject.AddComponent<SliderJoint>();
        s.axis = Vector3.forward;
        s.useLimits = true;
        s.minDistance = 0f;
        s.maxDistance = 0.6f;
    }
}
}
