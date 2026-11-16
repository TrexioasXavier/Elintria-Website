namespace DocCheck_StdBoxVisual
{
using Elintria;
using Elintria.Standard;

public class DropCoin : ElintriaScript
{
    public void OnDeath()
    {
        GameObject coin = StdBoxVisual.Make("Coin", transform.position + Vector3.up * 0.3f, new Vector3(0.2f, 0.2f, 0.05f), new Color(1f, 0.8f, 0.2f, 1f));
        Pickup p = coin.AddComponent<Pickup>();
        p.itemId = "coin";
        p.autoPickup = true;
    }
}
}
