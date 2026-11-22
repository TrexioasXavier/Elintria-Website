namespace DocCheck_ConvexShape
{
using Elintria;

public class RockHull : ElintriaScript
{
    public override void Start()
    {
        ConvexCollider c = gameObject.AddComponent<ConvexCollider>();
        c.shape = ConvexShape.ModelHull;
        c.meshPath = "Assets/Models/Rock.obj";
        c.maxVertices = 32;
    }
}
}
