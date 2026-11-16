namespace DocCheck_SkinnedModel
{
using Elintria;

public class HeadTurn : ElintriaScript
{
    public GameObject target;
    public float weight = 0.8f;

    public override void LateUpdate()
    {
        SkinnedModel m = SkinnedModel.Get(gameObject);
        if (m == null || !m.hasBones) return;
        if (target == null) { m.ClearBoneOverrides("Head"); return; }
        if (m.AnimatedBonePose("Head", out Vector3 head, out Quaternion _))
            m.SetBoneRotation("Head", Quaternion.LookRotation(target.transform.position - head), weight);
    }
}
}
