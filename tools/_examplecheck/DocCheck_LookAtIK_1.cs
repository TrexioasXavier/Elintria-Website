namespace DocCheck_LookAtIK
{
using Elintria;
using Elintria.Standard;

public class LookAtPoint : ElintriaScript
{
    public override void Start()
    {
        LookAtIK ik = GetComponent<LookAtIK>();
        ik.useTargetPoint = true;
        ik.targetPoint = new Vector3(10f, 2f, 5f);
        ik.maxAngle = 60f;
        ik.spineShare = 0.3f;
        ik.neckShare = 0.3f;
    }
}
}
