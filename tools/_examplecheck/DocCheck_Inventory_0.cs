namespace DocCheck_Inventory
{
using Elintria;
using Elintria.Standard;

public class PotionUser : ElintriaScript
{
    public void OnUseItem(string id)
    {
        Inventory inv = GetComponent<Inventory>();
        Health h = GetComponent<Health>();
        if (id == "potion" && h.health < h.maxHealth && inv.Remove("potion") == 1)
            h.Heal(25f);
    }
}
}
