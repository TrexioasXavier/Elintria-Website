namespace DocCheck_MoneyPickup
{
using Elintria;
using Elintria.Standard;

public class CoinTrail : ElintriaScript
{
    public override void Start()
    {
        for (int i = 0; i < 10; ++i)
        {
            GameObject c = StdBoxVisual.Make("Coin", transform.position + transform.forward * i * 1.5f + Vector3.up * 0.4f, new Vector3(0.2f, 0.2f, 0.05f), new Color(1f, 0.8f, 0.2f, 1f));
            c.AddComponent<MoneyPickup>().amount = 1;
        }
    }
}
}
