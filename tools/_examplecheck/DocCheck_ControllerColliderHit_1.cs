namespace DocCheck_ControllerColliderHit
{
using Elintria;

public class FootstepSurface : ElintriaScript
{
    public string surface = "default";

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.5f) surface = hit.gameObject.tag;   // ground under the feet
    }
}
}
