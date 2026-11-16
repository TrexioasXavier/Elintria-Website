namespace DocCheck_CraftingBench
{
using Elintria;
using Elintria.Standard;

public class Forge : ElintriaScript
{
    public override void Start()
    {
        CraftingBench b = GetComponent<CraftingBench>();
        b.benchName = "Forge";
        b.recipes = "sword:1=iron:3+wood:1|helmet:1=iron:4|nails:10=iron:1";
        b.craftTime = 2f;
    }
}
}
