namespace DocCheck_LootChest
{
using Elintria;
using Elintria.Standard;

public class RefillChest : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        if (d != null && d.hour < 0.05f) GetComponent<LootChest>().opened = false;
    }
}
}
