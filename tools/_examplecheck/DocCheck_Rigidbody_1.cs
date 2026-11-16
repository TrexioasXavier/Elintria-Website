namespace DocCheck_Rigidbody
{
using Elintria;

public class BallJump : ElintriaScript
{
    public float jumpImpulse = 6f;
    public float moveForce = 10f;
    Rigidbody rb;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public override void Update()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);
        if (grounded && Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector3.up * jumpImpulse * rb.mass, ForceMode.Impulse);
    }

    public override void FixedUpdate()
    {
        rb.AddForce(new Vector3(Input.GetAxisHorizontal(), 0f, Input.GetAxisVertical()) * moveForce);
    }
}
}
