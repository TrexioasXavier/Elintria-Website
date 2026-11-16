namespace DocCheck_Recipe
{
using Elintria;
using Elintria.Standard;

public class CraftableList : ElintriaScript
{
    public CraftingBench bench;

    public override void Update()
    {
        float y = 300f;
        foreach (Recipe r in Recipe.Parse(bench.recipes))
            if (r.CanMake(GetComponent<Inventory>())) { ScreenGUI.Text(20f, y, r.result + " (" + r.Describe() + ")"); y += 20f; }
    }
}
}
