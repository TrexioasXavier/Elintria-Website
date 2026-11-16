namespace DocCheck_StdInput
{
using Elintria;
using Elintria.Standard;

public class AutoTurret : ElintriaScript
{
    public float every = 0.5f;
    float next;

    public override void Update()
    {
        Weapon gun = GetComponent<HitscanWeapon>();
        gun.input.overridden = true;
        gun.input.fire = Time.time >= next;
        gun.input.fireHeld = gun.input.fire;
        if (gun.input.fire) next = Time.time + every;
    }
}
}
