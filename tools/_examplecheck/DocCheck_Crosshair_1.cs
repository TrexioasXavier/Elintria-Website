namespace DocCheck_Crosshair
{
using Elintria;
using Elintria.Standard;

public class AimHidesCrosshair : ElintriaScript
{
    public override void Update()
    {
        GetComponent<Crosshair>().enabled = !Input.GetMouseButton(1);
    }
}
}
