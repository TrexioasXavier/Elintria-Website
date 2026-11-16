namespace DocCheck_DayNightCycle
{
using Elintria;
using Elintria.Standard;

public class LongDay : ElintriaScript
{
    public override void Start() { GetComponent<DayNightCycle>().dayMinutes = 40f; }
}
}
