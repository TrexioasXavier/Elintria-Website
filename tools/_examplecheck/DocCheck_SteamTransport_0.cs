namespace DocCheck_SteamTransport
{
using Elintria;
using Elintria.Netcode;

public class SteamId : ElintriaScript
{
    SteamTransport steam = new SteamTransport();

    public override void Update()
    {
        ScreenGUI.Text(20, 20, steam.Unavailable == "" ? "Your SteamID: " + steam.MySteamId : "Steam: " + steam.Unavailable);
    }
}
}
