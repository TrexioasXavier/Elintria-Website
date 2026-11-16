namespace DocCheck_ServerRpcAttribute
{
using Elintria;
using Elintria.Netcode;

public class NetJump : NetworkBehaviour
{
    public override void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.Space)) CallServer(nameof(Jump));
    }

    [ServerRpc] void Jump() { GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse); }
}
}
