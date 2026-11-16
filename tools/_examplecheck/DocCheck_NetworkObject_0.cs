namespace DocCheck_NetworkObject
{
using Elintria;
using Elintria.Netcode;

public class PickupOwnership : NetworkBehaviour
{
    public override void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        NetworkObject player = other.gameObject.GetComponent<NetworkObject>();
        if (player != null) NetworkManager.SetOwner(NetworkObject, player.OwnerId);   // now that player carries it
    }
}
}
