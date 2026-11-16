namespace DocCheck_HudMarker
{
using Elintria;
using Elintria.Standard;

public class BlinkMarker : ElintriaScript
{
    public override void Update()
    {
        GetComponent<HudMarker>().onScreen = Mathf.Sin(Time.time * 6f) > 0f;
    }
}
}
