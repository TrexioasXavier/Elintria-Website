namespace DocCheck_Collider
{
using Elintria;

public class Magnet : ElintriaScript
{
    public GameObject plate;   // has a collider
    public float pull = 12f;

    public override void FixedUpdate()
    {
        Collider c = plate.GetComponent<Collider>();
        Rigidbody rb = GetComponent<Rigidbody>();
        if (c == null || rb == null) return;
        Vector3 to = c.ClosestPoint(transform.position) - transform.position;
        if (to.magnitude < 5f) rb.AddForce(to.normalized * pull);
    }
}
}
