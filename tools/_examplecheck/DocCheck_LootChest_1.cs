namespace DocCheck_LootChest
{
using Elintria;
using Elintria.Standard;

public class Mimic : ElintriaScript
{
    public override void Update()
    {
        if (!GetComponent<LootChest>().opened) return;
        BasicEnemy e = GetComponent<BasicEnemy>();
        if (e != null && !e.enabled) { e.enabled = true; StdToast.Show("It's a mimic!"); }
    }
}
}
