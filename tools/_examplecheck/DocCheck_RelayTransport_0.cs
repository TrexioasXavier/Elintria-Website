namespace DocCheck_RelayTransport
{
using Elintria;
using Elintria.Netcode;

public class RelayRoomHost : ElintriaScript
{
    public string relayUrl = "wss://your-relay.onrender.com";

    public override void Start()
    {
        string room = "R" + Random.Range(10000, 99999);
        NetworkManager.StartHost(new RelayTransport(), new NetworkConfig { relayUrl = relayUrl, room = room, name = "Host" });
        Debug.Log("Room code: " + room);
    }
}
}
