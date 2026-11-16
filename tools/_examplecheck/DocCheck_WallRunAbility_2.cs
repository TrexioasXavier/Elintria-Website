namespace DocCheck_WallRunAbility
{
using Elintria;
using Elintria.Standard;

public class WallRunTilt : ElintriaScript
{
    public override void Update()
    {
        if (GetComponent<WallRunAbility>().running) StdCameraShake.Shake(0.02f, 0.1f, 4f);
    }
}
}
