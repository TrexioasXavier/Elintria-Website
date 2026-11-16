namespace DocCheck_CollisionDetectionMode
{
using Elintria;

public class FastBody : ElintriaScript
{
    public override void Start() { GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative; }
}
}
