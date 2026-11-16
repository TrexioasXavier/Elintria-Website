namespace DocCheck_Needs
{
using Elintria;
using Elintria.Standard;

public class FoodList : ElintriaScript
{
    public override void Start()
    {
        Needs n = GetComponent<Needs>();
        n.consumables = "apple:hunger=25|stew:hunger=50,warmth=10|coffee:thirst=20,stamina=40";
    }
}
}
