namespace DocCheck_Crosshair
{
using Elintria;
using Elintria.Standard;

public class ShotgunCrosshair : ElintriaScript
{
    public override void Update()
    {
        HitscanWeapon w = GetComponent<HitscanWeapon>();
        GetComponent<Crosshair>().gap = w != null && w.pellets > 1 ? 18f : 6f;
    }
}
}
