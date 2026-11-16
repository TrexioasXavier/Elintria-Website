namespace DocCheck_LootTable
{
using Elintria;
using Elintria.Standard;

public class DailyReward : ElintriaScript
{
    public void Claim(Inventory inv)
    {
        foreach (ItemStack s in LootTable.Roll("gem:1:1:1|potion:3:1:3|coin:6:10:30")) inv.Add(s.id, s.count);
    }
}
}
