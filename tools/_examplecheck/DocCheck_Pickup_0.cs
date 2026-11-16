namespace DocCheck_Pickup
{
using Elintria;
using Elintria.Standard;

public class CoinSetup : ElintriaScript
{
    public override void Start()
    {
        Pickup p = GetComponent<Pickup>();
        p.itemId = "coin";
        p.count = 1;
        p.autoPickup = true;
        p.autoPickupRadius = 1f;
        p.spin = true;
        p.respawnTime = 30f;
    }
}
}
