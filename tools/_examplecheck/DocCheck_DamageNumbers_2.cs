namespace DocCheck_DamageNumbers
{
using Elintria;
using Elintria.Standard;

public class SlowNumbers : ElintriaScript
{
    public override void Start()
    {
        DamageNumbers d = GetComponent<DamageNumbers>();
        d.riseSpeed = 0.6f;
        d.lifetime = 2f;
    }
}
}
