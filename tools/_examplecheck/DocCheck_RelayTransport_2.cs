namespace DocCheck_RelayTransport
{
using Elintria;
using Elintria.Netcode;

public class AnyJoin : ElintriaScript
{
    public string relayUrl = "wss://your-relay.onrender.com";

    public void Join(string room)
    {
        var lan = LanTransport.Discover(500);
        if (lan.Count > 0) NetworkManager.StartClient(new LanTransport(), new NetworkConfig { address = lan[0].address, port = lan[0].port });
        else NetworkManager.StartClient(new RelayTransport(), new NetworkConfig { relayUrl = relayUrl, room = room });
    }
}
}
