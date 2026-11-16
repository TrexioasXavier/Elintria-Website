namespace DocCheck_NetReader
{
using Elintria;
using Elintria.Netcode;

public static class SkipHeader
{
    public static Vector3 PositionOf(string message)
    {
        var r = new NetReader(message.Split('|'), 1);   // token 0 is the message type
        return r.Value<Vector3>();
    }
}
}
