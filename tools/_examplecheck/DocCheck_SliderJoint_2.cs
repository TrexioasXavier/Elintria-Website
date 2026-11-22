namespace DocCheck_SliderJoint
{
using Elintria;

public class LiftFloor : ElintriaScript
{
    public float floorHeight = 4f;

    public override void Update()
    {
        SliderJoint s = GetComponent<SliderJoint>();
        if (s != null) Debug.Log("Floor " + Mathf.RoundToInt(s.position / floorHeight));
    }
}
}
