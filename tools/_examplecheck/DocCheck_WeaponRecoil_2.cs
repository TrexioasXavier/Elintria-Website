namespace DocCheck_WeaponRecoil
{
using Elintria;
using Elintria.Standard;

public class HeavyGun : ElintriaScript
{
    public override void Start()
    {
        WeaponRecoil r = GetComponent<WeaponRecoil>();
        r.kickUp = 2f;
        r.kickSide = 1f;
        r.recovery = 0.4f;
    }
}
}
