namespace DocCheck_NetworkObject
{
using Elintria;
using Elintria.Netcode;

public class MineMarker : ElintriaScript
{
    public override void Update()
    {
        NetworkObject n = GetComponent<NetworkObject>();
        if (n != null && n.IsSpawned) ScreenGUI.Text(20, 20, "Object #" + n.NetId + (n.IsOwner ? " (yours)" : " owned by " + n.OwnerId));
    }
}
}
