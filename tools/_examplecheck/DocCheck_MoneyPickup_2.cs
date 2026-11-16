namespace DocCheck_MoneyPickup
{
using Elintria;
using Elintria.Standard;

public class MoneyBag : ElintriaScript
{
    public override void Start() { gameObject.AddComponent<MoneyPickup>().amount = 100; }
}
}
