namespace DocCheck_Vendor
{
using Elintria;
using Elintria.Standard;

public class RisingPrices : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        GetComponent<Vendor>().stock = d != null && !d.isDay ? "potion:40|torch:16" : "potion:20|torch:8";
    }
}
}
