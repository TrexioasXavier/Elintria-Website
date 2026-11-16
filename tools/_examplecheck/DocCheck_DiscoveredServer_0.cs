namespace DocCheck_DiscoveredServer
{
using Elintria;

public class ServerList : ElintriaScript
{
    public override void Update()
    {
        foreach (DiscoveredServer s in NetworkSession.DiscoverPoll())
            Debug.Log(s.name + " " + s.players + "/" + s.maxPlayers + " at " + s.address + ":" + s.port);
    }
}
}
