namespace DocCheck_FallDamage
{
using Elintria;
using Elintria.Standard;

public class SafeDrop : ElintriaScript
{
    public override void Start()
    {
        // the level starts with a fall from the sky
        FallDamage f = GetComponent<FallDamage>();
        if (f != null) f.Forgive();
    }
}
}
