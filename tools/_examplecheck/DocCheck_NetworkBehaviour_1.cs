namespace DocCheck_NetworkBehaviour
{
using Elintria;
using Elintria.Netcode;

public class NetMover : NetworkBehaviour
{
    public float speed = 5f;

    public override void Update()
    {
        if (!IsOwner) return;
        Vector3 move = new Vector3(Input.GetAxisHorizontal(), 0f, Input.GetAxisVertical());
        transform.position += move * speed * Time.deltaTime;
    }
}
}
