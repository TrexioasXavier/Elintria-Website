namespace DocCheck_UIRuntime
{
using Elintria;

public class DesignedHud : ElintriaScript
{
    public float health = 100f;
    public int coins;

    public override void Update()
    {
        UIRuntime.SetFill("HealthBar", health / 100f);
        UIRuntime.SetText("CoinText", coins.ToString());
        UIRuntime.SetProp("HealthBar", "color", health < 25f ? "1,0.2,0.2,1" : "0.2,0.9,0.3,1");
    }
}
}
