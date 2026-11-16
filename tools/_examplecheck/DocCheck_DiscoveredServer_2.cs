namespace DocCheck_DiscoveredServer
{
using Elintria;

public static class JoinServer
{
    public static bool Join(DiscoveredServer s) => NetworkSession.ConnectClient(s.address, s.port);
}
}
