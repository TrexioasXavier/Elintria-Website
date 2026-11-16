namespace DocCheck_ThirdPersonController
{
using Elintria;
using Elintria.Standard;

public class AimMode : ElintriaScript
{
    float normalFov;

    public override void Start() { normalFov = GetComponent<ThirdPersonController>().fieldOfView; }

    public override void Update()
    {
        ThirdPersonController tpc = GetComponent<ThirdPersonController>();
        bool aiming = Input.GetMouseButton(1);
        tpc.strafeMode = aiming;
        tpc.shoulderOffset = aiming ? 0.7f : 0.3f;
        tpc.fieldOfView = Mathf.Lerp(tpc.fieldOfView, aiming ? normalFov * 0.7f : normalFov, 10f * Time.deltaTime);
    }
}
}
