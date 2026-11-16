namespace DocCheck_LootChest
{
using Elintria;
using Elintria.Standard;

public class LockedChest : ElintriaScript
{
    public override void Start()
    {
        LootChest c = GetComponent<LootChest>();
        c.requiredItem = "chest_key";
        c.loot = "gem:1:2:3|money:1:50:100";
    }
}
}
