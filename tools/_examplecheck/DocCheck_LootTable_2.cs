namespace DocCheck_LootTable
{
using Elintria;
using Elintria.Standard;

public class DepthLoot : ElintriaScript
{
    public override void Start()
    {
        bool deep = transform.position.y < -20f;
        GetComponent<LootChest>().loot = deep ? "gem:4:1:2|relic:1:1:1" : "coin:6:3:10|potion:3:1:2";
    }
}
}
