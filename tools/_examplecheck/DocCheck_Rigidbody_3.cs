namespace DocCheck_Rigidbody
{
using Elintria;

public class HoverCraft : ElintriaScript
{
    public float maxSpeed = 15f;
    Rigidbody rb;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.8f;
        rb.angularDrag = 2f;
    }

    public override void FixedUpdate()
    {
        rb.AddForce(transform.forward * Input.GetAxisVertical() * 30f);
        rb.AddTorque(Vector3.up * Input.GetAxisHorizontal() * 5f);
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}
}
