namespace DocCheck_MoneyPickup
{
using Elintria;
using Elintria.Standard;

public class CoinMagnet : ElintriaScript
{
    public override void Start()
    {
        foreach (MoneyPickup m in FindObjectsOfType<MoneyPickup>()) m.radius = 4f;
    }
}
}
