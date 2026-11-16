namespace DocCheck_INetTransport
{
using Elintria;
using Elintria.Netcode;

public class PickTransport : ElintriaScript
{
    public static INetTransport Best()
    {
        INetTransport steam = NetworkManager.MakeTransport("steam");
        if (steam.Unavailable == "") return steam;
        Debug.Log("Steam unavailable: " + steam.Unavailable);
        return NetworkManager.MakeTransport("lan");
    }
}
}
