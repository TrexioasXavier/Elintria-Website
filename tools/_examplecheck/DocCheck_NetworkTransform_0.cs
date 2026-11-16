namespace DocCheck_NetworkTransform
{
using Elintria;
using Elintria.Netcode;

public class NetPlatform : NetworkBehaviour
{
    Vector3 start;
    public override void Start() { start = transform.position; }

    public override void Update()
    {
        if (IsServer) transform.position = start + Vector3.right * Mathf.Sin(Time.time) * 4f;
    }
}
}
