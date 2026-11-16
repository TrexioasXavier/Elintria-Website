namespace DocCheck_LanTransport
{
using System.Collections.Generic;
using Elintria;
using Elintria.Netcode;

public class LanBrowser : ElintriaScript
{
    List<DiscoveredGame> games = new List<DiscoveredGame>();

    public override void Update()
    {
        if (ScreenGUI.Button(new Rect(20, 20, 160, 36), "Refresh")) games = LanTransport.Discover(800);
        for (int i = 0; i < games.Count; i++)
        {
            DiscoveredGame g = games[i];
            if (ScreenGUI.Button(new Rect(20, 70 + i * 40, 360, 36), g.name + "  " + g.players + "/" + g.maxPlayers))
                NetworkManager.StartClient(new LanTransport(), new NetworkConfig { address = g.address, port = g.port, name = "Player" });
        }
    }
}
}
