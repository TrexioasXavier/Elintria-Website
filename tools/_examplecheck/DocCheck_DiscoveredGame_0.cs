namespace DocCheck_DiscoveredGame
{
using Elintria;
using Elintria.Netcode;

public class OpenGames : ElintriaScript
{
    public override void Start()
    {
        foreach (DiscoveredGame g in LanTransport.Discover())
            if (g.players < g.maxPlayers) Debug.Log("Open: " + g.name + " at " + g.address + ":" + g.port);
    }
}
}
