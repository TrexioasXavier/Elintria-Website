namespace DocCheck_NetworkManagerComponent
{
using Elintria;
using Elintria.Netcode;

public class RelayHost : ElintriaScript
{
    public override void Start()
    {
        var net = GetComponent<NetworkManagerComponent>();
        net.transport = "relay";
        net.roomCode = "ROOM" + Random.Range(1000, 9999);
        Debug.Log("Tell your friends the room code: " + net.roomCode);
        net.StartHost();
    }
}
}
