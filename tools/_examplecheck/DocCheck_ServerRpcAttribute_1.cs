namespace DocCheck_ServerRpcAttribute
{
using Elintria;
using Elintria.Netcode;

public class NetDoor : NetworkBehaviour
{
    public NetworkVariable<bool> open = new NetworkVariable<bool>();

    public void Use() { CallServer(nameof(Toggle)); }

    [ServerRpc(RequireOwnership = false)] void Toggle() { open.Value = !open.Value; }

    public override void Update()
    {
        transform.localEulerAngles = new Vector3(0f, open ? 90f : 0f, 0f);
    }
}
}
