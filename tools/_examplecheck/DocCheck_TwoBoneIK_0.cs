namespace DocCheck_TwoBoneIK
{
using Elintria;
using Elintria.Standard;

public class HandOnHandle : ElintriaScript
{
    public GameObject handle;

    public override void Start()
    {
        TwoBoneIK ik = GetComponent<TwoBoneIK>();
        ik.upperBone = "mixamorig:RightArm";
        ik.middleBone = "mixamorig:RightForeArm";
        ik.endBone = "mixamorig:RightHand";
        ik.target = handle;
        ik.matchRotation = true;
    }
}
}
