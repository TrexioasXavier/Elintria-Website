namespace DocCheck_Rigidbody
{
using Elintria;

public class KinematicLift : ElintriaScript
{
    public float height = 5f;
    public float period = 6f;
    Rigidbody rb;
    Vector3 start;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        start = transform.position;
    }

    public override void FixedUpdate()
    {
        float t = Mathf.PingPong(Time.time / period * 2f, 1f);
        rb.MovePosition(start + Vector3.up * Mathf.SmoothStep(0f, height, t));
    }
}
}
