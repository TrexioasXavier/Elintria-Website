namespace DocCheck_UIBridge
{
using Elintria;

public class HealthReadout : ElintriaScript
{
    public override void Update()
    {
        // The same path a Text element's Data Binding would use.
        string hp = UIBridge.Read("Player/Health.health");
        if (hp != "") UIRuntime.SetText("HealthLabel", "HP " + hp);
    }
}
}
