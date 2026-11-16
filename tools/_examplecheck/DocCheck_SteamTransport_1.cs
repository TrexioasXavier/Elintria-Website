namespace DocCheck_SteamTransport
{
using Elintria;
using Elintria.Netcode;

public class SteamHost : ElintriaScript
{
    public override void Start()
    {
        var steam = new SteamTransport();
        if (steam.Unavailable != "") { Debug.LogWarning(steam.Unavailable); return; }
        NetworkManager.StartHost(steam, new NetworkConfig { maxPlayers = 4, name = "Host" });
    }
}
}
