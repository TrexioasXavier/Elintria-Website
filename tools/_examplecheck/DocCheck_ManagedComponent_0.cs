namespace DocCheck_ManagedComponent
{
using System.Collections.Generic;
using Elintria;

public class LootTable : ManagedComponent
{
    public List<string> items = new List<string>();
    public float dropChance = 0.5f;
}

public class EnemySetup : ElintriaScript
{
    public override void Start()
    {
        LootTable loot = gameObject.AddComponent<LootTable>();
        loot.items.Add("coin");
        loot.items.Add("potion");
    }

    public override void OnDestroy()
    {
        LootTable loot = GetComponent<LootTable>();
        if (loot != null && Random.value < loot.dropChance)
            Debug.Log("Dropped " + loot.items[Random.Range(0, loot.items.Count)]);
    }
}
}
