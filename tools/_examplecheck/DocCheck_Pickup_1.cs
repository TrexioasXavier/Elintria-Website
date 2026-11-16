namespace DocCheck_Pickup
{
using Elintria;
using Elintria.Standard;

public class LootBag : ElintriaScript
{
    public GameObject pickupTemplate;   // inactive, with a Pickup
    public string item = "ammo";
    public int amount = 12;

    public void OnDeath()
    {
        GameObject bag = Instantiate(pickupTemplate, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Pickup p = bag.GetComponent<Pickup>();
        p.itemId = item;
        p.count = amount;
        p.respawnTime = 0f;
        bag.SetActive(true);
    }
}
}
