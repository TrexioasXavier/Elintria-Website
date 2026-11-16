namespace DocCheck_WeaponRecoil
{
using Elintria;
using Elintria.Standard;

public class CrouchSteady : ElintriaScript
{
    public override void Update()
    {
        FirstPersonController fp = GetComponent<FirstPersonController>();
        GetComponent<WeaponRecoil>().kickUp = fp.crouching ? 0.6f : 1.2f;
    }
}
}
