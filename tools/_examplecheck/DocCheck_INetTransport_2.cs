namespace DocCheck_INetTransport
{
using Elintria;
using Elintria.Netcode;

public class RawChat : ElintriaScript
{
    INetTransport t;

    public override void Start()
    {
        t = new LanTransport();
        t.StartHost(new NetworkConfig { port = 7790, maxPlayers = 4 });
    }

    public override void Update()
    {
        while (t.Poll(out NetEvent e))
            if (e.kind == NetEventKind.Data) { Debug.Log("peer " + e.peer + ": " + e.data); t.Send(0, e.data); }
    }

    public override void OnDestroy() { t.Stop(); }
}
}
