namespace DocCheck_Sprinkler
{
using Elintria;
using Elintria.Standard;

public class MorningSprinkler : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        if (d != null) GetComponent<Sprinkler>().on = d.hour >= 6f && d.hour < 8f;
    }
}
}
