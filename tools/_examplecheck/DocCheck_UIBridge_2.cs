namespace DocCheck_UIBridge
{
using Elintria;

public class StatusPanel : ElintriaScript
{
    public override void Update()
    {
        string[] v = UIBridge.ReadAll("Player/Health.health\nPlayer/Health.maxHealth\nPlayer/Wallet.money").Split('\n');
        if (v.Length < 3) return;
        UIRuntime.SetText("StatusText", "HP " + v[0] + " / " + v[1] + "   Coins " + v[2]);
    }
}
}
