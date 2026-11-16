namespace DocCheck_ClientRpcAttribute
{
using Elintria;
using Elintria.Netcode;

public class NetExplosion : NetworkBehaviour
{
    public void Explode()
    {
        if (IsServer) CallClients(nameof(PlayEffect), transform.position);
    }

    [ClientRpc] void PlayEffect(Vector3 at) { Audio.PlayClipAtPoint("Audio/boom.wav", at); }
}
}
