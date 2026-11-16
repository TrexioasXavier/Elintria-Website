namespace DocCheck_NetEventKind
{
using Elintria.Netcode;

public static class KindName
{
    public static string Of(NetEventKind k)
    {
        switch (k)
        {
            case NetEventKind.Connected: return "joined";
            case NetEventKind.Disconnected: return "left";
            case NetEventKind.Data: return "message";
            default: return "error";
        }
    }
}
}
