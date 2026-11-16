namespace DocCheck_Vendor
{
using Elintria;
using Elintria.Standard;

public class LimitedStock : ElintriaScript
{
    public override void Start() { GetComponent<Vendor>().stockCount = 3; }
}
}
