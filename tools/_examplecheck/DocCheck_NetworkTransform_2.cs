namespace DocCheck_NetworkTransform
{
using Elintria;
using Elintria.Netcode;

public class NetTeleport : NetworkBehaviour
{
    public void TeleportTo(Vector3 p)
    {
        if (!IsServer) return;
        NetworkTransform nt = GetComponent<NetworkTransform>();
        nt.interpolate = false;
        transform.position = p;
        Invoke("SmoothAgain", 0.2f);
    }

    void SmoothAgain() { GetComponent<NetworkTransform>().interpolate = true; }
}
}
