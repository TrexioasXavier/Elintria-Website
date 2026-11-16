namespace DocCheck_Rigidbody
{
using Elintria;

public class Rocket : ElintriaScript
{
    public float thrust = 25f;
    public float turn = 4f;
    Rigidbody rb;

    public override void Start() { rb = GetComponent<Rigidbody>(); }

    public override void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space)) rb.AddRelativeForce(Vector3.up * thrust);
        rb.AddTorque(new Vector3(Input.GetAxisVertical(), 0f, -Input.GetAxisHorizontal()) * turn);
    }
}
}
