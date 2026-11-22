namespace DocCheck_ConvexShape
{
using Elintria;

public class ShapedProp : ElintriaScript
{
    public int shapeIndex = 1;      // 1 box, 2 cylinder, 3 capsule, 4 cone, 5 sphere

    public override void Start()
    {
        ConvexCollider c = gameObject.AddComponent<ConvexCollider>();
        c.shape = (ConvexShape)shapeIndex;
        gameObject.AddComponent<Rigidbody>();
    }
}
}
