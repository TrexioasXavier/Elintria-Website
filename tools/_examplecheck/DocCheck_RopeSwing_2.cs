namespace DocCheck_RopeSwing
{
using Elintria;
using Elintria.Standard;

public class RopePush : ElintriaScript
{
    public RopeSwing rope;

    public void Push()
    {
        if (rope.swinging) rope.swingVelocity += transform.forward * 6f;
    }
}
}
