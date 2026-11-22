namespace DocCheck_CompoundCollider
{
using Elintria;

public class Sled : ElintriaScript
{
    public override void Start()
    {
        CompoundCollider c = GetComponent<CompoundCollider>();
        if (c != null) { c.friction = 0.05f; c.bounciness = 0f; }
    }
}
}
