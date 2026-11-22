namespace DocCheck_ConvexCollider
{
using Elintria;

public class Barrel : ElintriaScript
{
    public override void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        ConvexCollider c = gameObject.AddComponent<ConvexCollider>();
        c.shape = ConvexShape.Cylinder;
        c.size = new Vector3(1f, 1.4f, 1f);
        c.friction = 0.8f;
        gameObject.AddComponent<Rigidbody>().mass = 20f;
    }
}
}
