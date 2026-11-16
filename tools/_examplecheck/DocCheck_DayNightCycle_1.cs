namespace DocCheck_DayNightCycle
{
using Elintria;
using Elintria.Standard;

public class StreetLamp : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        Light l = GetComponent<Light>();
        if (d != null && l != null) l.intensity = d.isDay ? 0f : 1.5f;
    }
}
}
