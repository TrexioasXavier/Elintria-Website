namespace DocCheck_TwoBoneIK
{
using Elintria;
using Elintria.Standard;

public class ReachWhenNear : ElintriaScript
{
    public override void Update()
    {
        TwoBoneIK ik = GetComponent<TwoBoneIK>();
        if (ik.target == null) return;
        float d = Vector3.Distance(ik.target.transform.position, transform.position + Vector3.up * 1.3f);
        ik.weight = Mathf.MoveTowards(ik.weight, d < 0.8f ? 1f : 0f, Time.deltaTime * 3f);
    }
}
}
