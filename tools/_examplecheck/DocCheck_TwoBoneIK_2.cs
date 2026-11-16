namespace DocCheck_TwoBoneIK
{
using Elintria;
using Elintria.Standard;

public class ReachCheck : ElintriaScript
{
    public override void Update()
    {
        TwoBoneIK ik = GetComponent<TwoBoneIK>();
        if (ik.weight > 0.9f && ik.reachError > 0.1f) Debug.Log("Target is " + ik.reachError.ToString("0.00") + " m out of reach");
    }
}
}
