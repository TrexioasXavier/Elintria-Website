namespace DocCheck_DamageResistance
{
using Elintria;
using Elintria.Standard;

public class FireElemental : ElintriaScript
{
    public override void Start()
    {
        DamageResistance r = gameObject.AddComponent<DamageResistance>();
        r.fire = 0f;
        r.cold = 2f;
    }
}
}
