namespace DocCheck_GravityZone
{
using Elintria;
using Elintria.Standard;

public class GravityFlip : ElintriaScript
{
    public GravityZone zone;
    public void Flip() { zone.gravityScale = zone.gravityScale > 0f ? -0.5f : 0.3f; }
}
}
