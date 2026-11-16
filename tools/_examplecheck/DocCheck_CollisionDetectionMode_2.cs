namespace DocCheck_CollisionDetectionMode
{
using Elintria;

public class ConvertedBullet : ElintriaScript
{
    public override void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;   // compiles; the solver stays discrete
        rb.velocity = transform.forward * 60f;
    }
}
}
