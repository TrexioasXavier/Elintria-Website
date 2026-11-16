namespace DocCheck_WeaponRecoil
{
using Elintria;
using Elintria.Standard;

public class PadAssist : ElintriaScript
{
    public bool usingPad;

    public override void Update()
    {
        GetComponent<WeaponRecoil>().aimAssist = usingPad ? 0.6f : 0f;
    }
}
}
