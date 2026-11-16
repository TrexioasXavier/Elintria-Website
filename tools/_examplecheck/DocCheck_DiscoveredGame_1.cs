namespace DocCheck_DiscoveredGame
{
using Elintria;
using Elintria.Netcode;

public class QuickJoin : ElintriaScript
{
    public override void Start()
    {
        var games = LanTransport.Discover(1000);
        if (games.Count == 0) { NetworkManager.StartHost(new LanTransport(), new NetworkConfig { port = 7777 }); return; }
        NetworkManager.StartClient(new LanTransport(), new NetworkConfig { address = games[0].address, port = games[0].port });
    }
}
}
