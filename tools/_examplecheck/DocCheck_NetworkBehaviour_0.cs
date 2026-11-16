namespace DocCheck_NetworkBehaviour
{
using Elintria;
using Elintria.Netcode;

public class NetShooter : NetworkBehaviour
{
    public override void Update()
    {
        if (IsOwner && Input.GetMouseButtonDown(0))
            CallServer(nameof(Shoot), transform.position, transform.forward);
    }

    [ServerRpc] void Shoot(Vector3 from, Vector3 dir)
    {
        if (Physics.Raycast(from, dir, out RaycastHit hit, 100f))
        {
            hit.gameObject.SendMessage("ApplyDamage", 20f);
            CallClients(nameof(ShowHit), hit.point);
        }
    }

    [ClientRpc] void ShowHit(Vector3 at) { Debug.DrawRay(at, Vector3.up, Color.red, 0.5f); }
}
}
