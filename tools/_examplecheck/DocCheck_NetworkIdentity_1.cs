namespace DocCheck_NetworkIdentity
{
using Elintria;

public class ServerProp : ElintriaScript
{
    public override void Start()
    {
        NetworkIdentity id = GetComponent<NetworkIdentity>();
        if (id != null && NetworkSession.IsServer) id.owner = NetworkIdentity.OwnerServer;
    }
}
}
