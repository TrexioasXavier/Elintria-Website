namespace DocCheck_ManagedComponent
{
using Elintria;

public class LootReader : ElintriaScript
{
    public GameObject chest;

    public override void Start()
    {
        if (chest != null && chest.TryGetComponent<LootTable>(out LootTable loot))
            Debug.Log(chest.name + " can drop " + loot.items.Count + " kinds of item");
    }
}
}
