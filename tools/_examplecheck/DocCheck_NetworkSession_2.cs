namespace DocCheck_NetworkSession
{
using Elintria;

public class SessionStatus : ElintriaScript
{
    public override void Update()
    {
        if (!NetworkSession.IsAvailable) { ScreenGUI.Text(20, 20, "No networking: " + NetworkSession.UnavailableReason); return; }
        ScreenGUI.Text(20, 20, NetworkSession.Role + "  ready " + NetworkSession.IsReady + "  peers " + NetworkSession.PeerCount +
                               "  ping " + NetworkSession.PingMs + "  address " + NetworkSession.LocalAddress);
    }
}
}
