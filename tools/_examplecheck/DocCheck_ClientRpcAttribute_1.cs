namespace DocCheck_ClientRpcAttribute
{
using Elintria;
using Elintria.Netcode;

public class Announcer2 : NetworkBehaviour
{
    string banner;
    float until;

    public void Announce(string text) { if (IsServer) CallClients(nameof(Show), text); }

    [ClientRpc] void Show(string text) { banner = text; until = Time.time + 3f; }

    public override void Update()
    {
        if (Time.time < until) ScreenGUI.Label(new Rect(0, 80, ScreenGUI.screenWidth, 40), banner, null, Color.yellow, TextAnchor.MiddleCenter);
    }
}
}
