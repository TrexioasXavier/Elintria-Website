namespace DocCheck_TwoBoneSolver
{
using Elintria;
using Elintria.Standard;

public class LegSolve : ElintriaScript
{
    public Vector3 footTarget;

    public override void LateUpdate()
    {
        SkinnedModel m = SkinnedModel.Get(gameObject);
        if (!m.AnimatedBonePose("LeftUpLeg", out Vector3 a, out Quaternion ra)) return;
        m.AnimatedBonePose("LeftLeg", out Vector3 b, out Quaternion rb);
        m.AnimatedBonePose("LeftFoot", out Vector3 c, out Quaternion _);
        TwoBoneSolver.Solve(a, b, c, ra, rb, footTarget, b + transform.forward, out Quaternion na, out Quaternion nb);
        m.SetBoneRotation("LeftUpLeg", na);
        m.SetBoneRotation("LeftLeg", nb);
    }
}
}
