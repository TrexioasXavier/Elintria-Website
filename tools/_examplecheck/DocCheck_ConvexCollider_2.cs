namespace DocCheck_ConvexCollider
{
using Elintria;

public class IceRamp : ElintriaScript
{
    public override void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 15f);
        ConvexCollider ramp = gameObject.AddComponent<ConvexCollider>();
        ramp.shape = ConvexShape.Box;
        ramp.size = new Vector3(8f, 0.3f, 3f);
        ramp.friction = 0.02f;
    }
}
}
