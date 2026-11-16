namespace DocCheck_NetworkConfig
{
using Elintria;
using Elintria.Netcode;

public class RelayConfig : ElintriaScript
{
    public static NetworkConfig Make(string room) =>
        new NetworkConfig { relayUrl = "wss://my-relay.example.com", room = room, name = "Player" };
}
}
