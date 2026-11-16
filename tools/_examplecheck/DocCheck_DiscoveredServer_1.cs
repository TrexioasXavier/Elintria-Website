namespace DocCheck_DiscoveredServer
{
using Elintria;

public static class NotFull
{
    public static bool CanJoin(DiscoveredServer s) => s.players < s.maxPlayers;
}
}
