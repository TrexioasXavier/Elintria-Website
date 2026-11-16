namespace DocCheck_FootIK
{
using Elintria;
using Elintria.Standard;

public class SmallFeet : ElintriaScript
{
    public override void Start()
    {
        FootIK ik = GetComponent<FootIK>();
        ik.footHeight = 0.04f;
        ik.rayHeight = 0.3f;
        ik.maxStepDown = 0.25f;
        ik.smoothing = 20f;
    }
}
}
