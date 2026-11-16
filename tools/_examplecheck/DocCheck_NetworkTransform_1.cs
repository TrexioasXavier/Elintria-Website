namespace DocCheck_NetworkTransform
{
using Elintria;
using Elintria.Netcode;

public class PetFollow : NetworkBehaviour
{
    public override void Start()
    {
        NetworkTransform nt = GetComponent<NetworkTransform>();
        if (nt != null) nt.clientAuthority = true;   // the owner moves it
    }

    public override void Update()
    {
        if (!IsOwner || NetworkManager.LocalPlayer == null) return;
        Vector3 target = NetworkManager.LocalPlayer.transform.position + Vector3.back * 2f;
        transform.position = Vector3.Lerp(transform.position, target, 3f * Time.deltaTime);
    }
}
}
