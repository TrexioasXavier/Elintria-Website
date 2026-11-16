namespace DocCheck_NetworkConfig
{
using Elintria;
using Elintria.Netcode;

public class SteamJoin : ElintriaScript
{
    public void Join(ulong friendSteamId)
    {
        var cfg = new NetworkConfig { steamHost = friendSteamId, name = "Player" };
        NetworkManager.StartClient(NetworkManager.MakeTransport("steam"), cfg);
    }
}
}
