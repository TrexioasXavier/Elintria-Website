namespace DocCheck_ItemDefinition
{
using Elintria;
using Elintria.Standard;

public class RegisterItems : ElintriaScript
{
    public override void Awake()
    {
        ItemDatabase.Register(new ItemDefinition { id = "gem", displayName = "Ruby", description = "Sells well.", maxStack = 20, category = "Treasure", color = Color.red });
        ItemDatabase.Register(new ItemDefinition { id = "key_gold", displayName = "Gold Key", maxStack = 1, category = "Key", color = Color.yellow });
    }
}
}
