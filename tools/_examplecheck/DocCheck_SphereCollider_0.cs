namespace DocCheck_SphereCollider
{
using Elintria;

public class MagnetPickup : ElintriaScript
{
    public float pickupRadius = 1.5f;

    public override void Start()
    {
        SphereCollider s = gameObject.AddComponent<SphereCollider>();
        s.radius = pickupRadius;
        s.isTrigger = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { other.gameObject.SendMessage("AddCoins", 1); Destroy(gameObject); }
    }
}
}
