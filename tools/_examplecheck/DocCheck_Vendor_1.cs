namespace DocCheck_Vendor
{
using Elintria;
using Elintria.Standard;

public class GemCollector : ElintriaScript
{
    public override void Start()
    {
        Vendor v = GetComponent<Vendor>();
        v.stock = "";
        v.buys = "gem:50|relic:200";
    }
}
}
