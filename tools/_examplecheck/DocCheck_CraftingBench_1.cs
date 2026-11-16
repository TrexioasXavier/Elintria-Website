namespace DocCheck_CraftingBench
{
using Elintria;
using Elintria.Standard;

public class LearnRecipe : ElintriaScript
{
    public CraftingBench bench;

    public void OnUseItem(string id)
    {
        if (id == "recipe_bomb" && !bench.recipes.Contains("bomb")) bench.recipes += "|bomb:1=powder:2+cloth:1";
    }
}
}
