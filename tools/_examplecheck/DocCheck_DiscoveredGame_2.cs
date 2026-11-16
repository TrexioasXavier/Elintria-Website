namespace DocCheck_DiscoveredGame
{
using System.Linq;
using Elintria;
using Elintria.Netcode;

public class BusiestGame : ElintriaScript
{
    public override void Start()
    {
        var best = LanTransport.Discover().OrderByDescending(g => g.players).FirstOrDefault();
        if (best.name != null) Debug.Log("Busiest: " + best.name);
    }
}
}
