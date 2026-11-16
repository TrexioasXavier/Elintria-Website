namespace DocCheck_MeleeWeapon
{
using Elintria;
using Elintria.Standard;

public class HammerSetup : ElintriaScript
{
    public override void Start()
    {
        MeleeWeapon m = GetComponent<MeleeWeapon>();
        m.damage = 60f;
        m.range = 2.5f;
        m.arc = 120f;
        m.windup = 0.45f;
        m.cooldown = 1.2f;
        m.knockback = 12f;
        m.swingClip = "attack_heavy";
    }
}
}
