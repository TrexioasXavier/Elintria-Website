namespace DocCheck_SphereCollider
{
using Elintria;

public class RollingBall : ElintriaScript
{
    public override void Start()
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);   // already has a SphereCollider
        ball.transform.position = transform.position + Vector3.up * 3f;
        Rigidbody rb = ball.AddComponent<Rigidbody>();
        rb.velocity = new Vector3(3f, 0f, 0f);
        Debug.Log("Ball radius " + ball.GetComponent<SphereCollider>().radius);
    }
}
}
