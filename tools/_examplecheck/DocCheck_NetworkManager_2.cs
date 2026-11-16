namespace DocCheck_NetworkManager
{
using Elintria;
using Elintria.Netcode;

public class JoinFeed : ElintriaScript
{
    public override void Start()
    {
        NetworkManager.OnClientConnected += OnJoin;
        NetworkManager.OnClientDisconnected += OnLeave;
        NetworkManager.OnStopped += why => Debug.Log("Session ended: " + why);
    }

    void OnJoin(int id) { Debug.Log(NetworkManager.NameOf(id) + " joined"); }
    void OnLeave(int id) { Debug.Log("Player " + id + " left"); }

    public override void OnDestroy()
    {
        NetworkManager.OnClientConnected -= OnJoin;
        NetworkManager.OnClientDisconnected -= OnLeave;
    }
}
}
