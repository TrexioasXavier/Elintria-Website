namespace DocCheck_SteamTransport
{
using Elintria;
using Elintria.Netcode;

public class SteamFriendJoin : ElintriaScript
{
    public string friendId = "76561198000000000";

    public override void Start()
    {
        if (ulong.TryParse(friendId, out ulong id))
            NetworkManager.StartClient(new SteamTransport(), new NetworkConfig { steamHost = id });
    }
}
}
