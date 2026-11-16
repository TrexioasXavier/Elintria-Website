namespace DocCheck_NetworkSession
{
using System.Collections.Generic;
using Elintria;

public class SessionFinder : ElintriaScript
{
    readonly List<DiscoveredServer> found = new List<DiscoveredServer>();

    public override void Start() { NetworkSession.DiscoverBegin(7800); }

    public override void Update()
    {
        found.AddRange(NetworkSession.DiscoverPoll());   // only new ones each call
        for (int i = 0; i < found.Count; i++)
            if (ScreenGUI.Button(new Rect(20, 20 + i * 40, 300, 36), found[i].name))
            {
                NetworkSession.DiscoverEnd();
                NetworkSession.ConnectClient(found[i].address, found[i].port);
            }
    }
}
}
