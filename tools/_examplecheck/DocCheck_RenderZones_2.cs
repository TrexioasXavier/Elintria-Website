namespace DocCheck_RenderZones
{
using Elintria;

public class ZoneDebug : ElintriaScript
{
    public override void Update()
    {
        ScreenGUI.Text(20, 20, "Zone " + RenderZones.ZoneAt(transform.position) + ", hidden " + RenderZones.HiddenLastFrame);
    }
}
}
