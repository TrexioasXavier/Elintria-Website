namespace DocCheck_INetTransport
{
using System.Collections.Generic;
using Elintria.Netcode;

public class LoopbackTransport : INetTransport
{
    readonly Queue<NetEvent> inbox = new Queue<NetEvent>();
    public string Name => "loopback";
    public string Unavailable => "";
    public int LocalId { get; private set; }

    public bool StartHost(NetworkConfig c) { LocalId = 1; return true; }
    public bool StartClient(NetworkConfig c) { return false; }   // single player only
    public void Stop() { LocalId = 0; inbox.Clear(); }
    public void Send(int peer, string data) { inbox.Enqueue(new NetEvent { kind = NetEventKind.Data, peer = 1, data = data }); }
    public bool Poll(out NetEvent e) { if (inbox.Count > 0) { e = inbox.Dequeue(); return true; } e = default(NetEvent); return false; }
}
}
