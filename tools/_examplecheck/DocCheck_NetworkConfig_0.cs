namespace DocCheck_NetworkConfig
{
using Elintria;
using Elintria.Netcode;

public class LanConfig : ElintriaScript
{
    public static NetworkConfig Make(string playerName) =>
        new NetworkConfig { address = "192.168.1.20", port = 7777, maxPlayers = 8, name = playerName, gameName = playerName + "'s game" };
}
}
