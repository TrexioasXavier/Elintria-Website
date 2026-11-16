namespace DocCheck_LootTable
{
using Elintria;
using Elintria.Standard;

public class EnemyDrops : ElintriaScript
{
    public string table = "ammo:5:5:15|potion:2:1:1|money:4:2:8|none:5";

    public void OnDeath()
    {
        foreach (ItemStack s in LootTable.Roll(table, 2)) LootTable.Drop(s.id, s.count, transform.position);
    }
}
}
