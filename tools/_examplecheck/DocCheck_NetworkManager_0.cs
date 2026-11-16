namespace DocCheck_NetworkManager
{
using Elintria;
using Elintria.Netcode;

public class NetMenu : ElintriaScript
{
    string address = "127.0.0.1";

    public override void Update()
    {
        if (NetworkManager.IsRunning)
        {
            ScreenGUI.Text(20, 20, NetworkManager.Mode + "  players " + NetworkManager.ConnectedClients.Count + "  ping " + NetworkManager.RttMs.ToString("0") + " ms");
            if (ScreenGUI.Button(new Rect(20, 50, 140, 36), "Leave")) NetworkManager.Stop("left");
            return;
        }
        var cfg = new NetworkConfig { address = address, port = 7777, maxPlayers = 4, name = "Player" };
        if (ScreenGUI.Button(new Rect(20, 50, 140, 36), "Host"))
            NetworkManager.StartHost(NetworkManager.MakeTransport("lan"), cfg);
        if (ScreenGUI.Button(new Rect(20, 92, 140, 36), "Join " + address))
            if (!NetworkManager.StartClient(NetworkManager.MakeTransport("lan"), cfg)) Debug.LogWarning(NetworkManager.LastError);
    }
}
}
