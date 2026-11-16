namespace DocCheck_NetworkMode
{
using Elintria;
using Elintria.Netcode;

public class NoHudOnServer : ElintriaScript
{
    public override void Update()
    {
        if (NetworkManager.Mode == NetworkMode.Server) return;   // nothing to draw
        ScreenGUI.Text(20, 20, "HUD");
    }
}
}
