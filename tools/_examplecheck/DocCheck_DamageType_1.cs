namespace DocCheck_DamageType
{
using Elintria;
using Elintria.Standard;

public class IceSpike : ElintriaScript
{
    public void Hit(GameObject target)
    {
        StdDamage.Apply(target, 15f, gameObject, target.transform.position, Vector3.up, 2f, DamageType.Cold);
    }
}
}
