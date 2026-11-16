namespace DocCheck_RelayTransport
{
using Elintria;
using Elintria.Netcode;

public class RelayRoomJoin : ElintriaScript
{
    public string relayUrl = "wss://your-relay.onrender.com";
    public string room = "R12345";

    public override void Start()
    {
        if (!NetworkManager.StartClient(new RelayTransport(), new NetworkConfig { relayUrl = relayUrl, room = room, name = "Guest" }))
            Debug.LogWarning(NetworkManager.LastError);
    }
}
}
