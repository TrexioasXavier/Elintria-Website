namespace DocCheck_NetworkObject
{
using Elintria;
using Elintria.Netcode;

public class Expire : NetworkBehaviour
{
    public float lifetime = 20f;
    float born;

    public override void Start() { born = Time.time; }

    public override void Update()
    {
        if (IsServer && Time.time - born > lifetime) NetworkManager.Despawn(NetworkObject);
    }
}
}
