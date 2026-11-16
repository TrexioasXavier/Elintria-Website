namespace DocCheck_FootIK
{
using Elintria;
using Elintria.Standard;

public class FootIkBySpeed : ElintriaScript
{
    public override void Update()
    {
        FootIK ik = GetComponent<FootIK>();
        ThirdPersonController tpc = GetComponent<ThirdPersonController>();
        float speed = tpc != null ? new Vector3(tpc.velocity.x, 0f, tpc.velocity.z).magnitude : 0f;
        ik.weight = Mathf.MoveTowards(ik.weight, speed < 1f && tpc.isGrounded ? 1f : 0f, Time.deltaTime * 4f);
    }
}
}
