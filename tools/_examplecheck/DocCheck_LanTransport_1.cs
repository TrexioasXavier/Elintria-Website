namespace DocCheck_LanTransport
{
using Elintria;
using Elintria.Netcode;

public class LanHost : ElintriaScript
{
    public override void Start()
    {
        bool ok = NetworkManager.StartHost(new LanTransport(), new NetworkConfig { port = 7777, maxPlayers = 6, gameName = "Friday Night" });
        Debug.Log(ok ? "Hosting on LAN" : "Failed: " + NetworkManager.LastError);
    }
}
}
