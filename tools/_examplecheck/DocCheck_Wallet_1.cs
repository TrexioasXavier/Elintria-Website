namespace DocCheck_Wallet
{
using Elintria;
using Elintria.Standard;

public class Credits : ElintriaScript
{
    public override void Start() { GetComponent<Wallet>().currency = "credits"; }
}
}
