namespace DocCheck_NetEventKind
{
using Elintria.Netcode;

public static class ErrorPolicy
{
    public static bool Lost(NetEvent e) => e.kind == NetEventKind.Disconnected || e.kind == NetEventKind.Error;
}
}
