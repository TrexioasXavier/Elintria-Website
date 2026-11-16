namespace DocCheck_NetworkBehaviour
{
using Elintria;
using Elintria.Netcode;

public class Whisper : NetworkBehaviour
{
    public void TellOwner(string text)
    {
        if (IsServer) CallClient(OwnerId, nameof(ShowMessage), text);
    }

    [ClientRpc] void ShowMessage(string text) { if (IsLocalPlayer) Debug.Log("Server says: " + text); }
}
}
