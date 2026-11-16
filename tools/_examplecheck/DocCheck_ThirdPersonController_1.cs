namespace DocCheck_ThirdPersonController
{
using Elintria;
using Elintria.Standard;

public class MoveModifiers : ElintriaScript
{
    public bool slowed;
    float walk, run;

    public override void Start()
    {
        var tpc = GetComponent<ThirdPersonController>();
        walk = tpc.walkSpeed; run = tpc.sprintSpeed;
    }

    public override void Update()
    {
        var tpc = GetComponent<ThirdPersonController>();
        bool inWater = Water.Depth(transform.position + Vector3.up * 0.5f) > 0f;
        float k = (slowed ? 0.5f : 1f) * (inWater ? 0.6f : 1f);
        tpc.walkSpeed = walk * k;
        tpc.sprintSpeed = run * k;
        tpc.jumpHeight = inWater ? 0.4f : 1.2f;
    }
}
}
