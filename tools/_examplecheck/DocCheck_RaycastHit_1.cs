namespace DocCheck_RaycastHit
{
using Elintria;

public class ForceGun : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 30f) && hit.rigidbody != null)
            hit.rigidbody.AddForce(transform.forward * 15f, ForceMode.Impulse);
    }
}
}
