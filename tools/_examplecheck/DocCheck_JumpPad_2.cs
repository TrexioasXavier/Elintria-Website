namespace DocCheck_JumpPad
{
using Elintria;
using Elintria.Standard;

public class PadPreview : ElintriaScript
{
    public override void Start()
    {
        JumpPad pad = GetComponent<JumpPad>();
        Vector3 v = pad.VelocityFrom(transform.position, 20f);
        Debug.Log("Launches at " + v.magnitude.ToString("0.0") + " m/s, " + (Mathf.Asin(v.normalized.y) * Mathf.Rad2Deg).ToString("0") + " degrees up");
    }
}
}
