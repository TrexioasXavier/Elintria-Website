namespace DocCheck_DamageType
{
using Elintria;
using Elintria.Standard;

public class Flamethrower : ElintriaScript
{
    public override void Start() { GetComponent<HitscanWeapon>().damageType = (int)DamageType.Fire; }
}
}
