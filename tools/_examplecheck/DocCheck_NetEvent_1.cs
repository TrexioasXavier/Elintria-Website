namespace DocCheck_NetEvent
{
using Elintria.Netcode;

public static class FakeEvents
{
    public static NetEvent Joined(int peer) => new NetEvent { kind = NetEventKind.Connected, peer = peer };
    public static NetEvent Said(int peer, string text) => new NetEvent { kind = NetEventKind.Data, peer = peer, data = text };
}
}
