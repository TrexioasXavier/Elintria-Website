namespace DocCheck_NetworkManagerComponent
{
using Elintria;
using Elintria.Netcode;

public class SimpleLobby : ElintriaScript
{
    public GameObject networkManagerObject;

    public override void Update()
    {
        if (NetworkManager.IsRunning) return;
        var net = networkManagerObject.GetComponent<NetworkManagerComponent>();
        if (ScreenGUI.Button(new Rect(40, 40, 160, 40), "Host")) net.StartHost();
        if (ScreenGUI.Button(new Rect(40, 90, 160, 40), "Join")) net.StartClient(net.address);
    }
}
}
