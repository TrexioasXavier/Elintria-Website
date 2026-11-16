namespace DocCheck_ClientRpcAttribute
{
using Elintria;
using Elintria.Netcode;

public class Kick : NetworkBehaviour
{
    public void Warn(int client) { CallClient(client, nameof(Warning), "Stop that!"); }
    [ClientRpc] void Warning(string text) { Debug.LogWarning(text); }
}
}
