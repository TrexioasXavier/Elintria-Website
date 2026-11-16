namespace DocCheck_NetworkIdentity
{
using Elintria;

public class IdLabel : ElintriaScript
{
    public override void Update()
    {
        NetworkIdentity id = GetComponent<NetworkIdentity>();
        if (id != null) ScreenGUI.Text(20, 60, name + " #" + id.networkId + " owner " + id.owner + (id.isPlayerSlot ? " (player)" : ""));
    }
}
}
