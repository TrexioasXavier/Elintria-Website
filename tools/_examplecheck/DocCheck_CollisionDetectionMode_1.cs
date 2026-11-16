namespace DocCheck_CollisionDetectionMode
{
using Elintria;

public class ModeReport : ElintriaScript
{
    public override void Start() { Debug.Log(GetComponent<Rigidbody>().collisionDetectionMode); }
}
}
