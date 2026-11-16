namespace DocCheck_LevelStreamer
{
using Elintria;

public class ZoneStats : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
            Debug.Log("Zone " + RenderZones.ZoneAt(transform.position) + ", hidden last frame: " + RenderZones.HiddenLastFrame);
    }
}
}
