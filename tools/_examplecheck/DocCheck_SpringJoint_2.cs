namespace DocCheck_SpringJoint
{
using Elintria;

public class StretchReadout : ElintriaScript
{
    public override void Update()
    {
        SpringJoint s = GetComponent<SpringJoint>();
        if (s != null) Debug.Log("Stretched to " + s.distance.ToString("0.00") + " m");
    }
}
}
