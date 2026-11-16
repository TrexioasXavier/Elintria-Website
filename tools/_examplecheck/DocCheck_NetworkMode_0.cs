namespace DocCheck_NetworkMode
{
using Elintria;
using Elintria.Netcode;

public class RoleBadge : ElintriaScript
{
    public override void Update() { ScreenGUI.Text(10, 10, "Mode: " + NetworkManager.Mode); }
}
}
