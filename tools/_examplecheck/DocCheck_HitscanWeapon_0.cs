namespace DocCheck_HitscanWeapon
{
using Elintria;
using Elintria.Standard;

public class ShotgunSetup : ElintriaScript
{
    public override void Start()
    {
        HitscanWeapon g = GetComponent<HitscanWeapon>();
        g.pellets = 8;
        g.spread = 6f;
        g.damage = 9f;          // per pellet
        g.fireRate = 1.2f;
        g.magazineSize = 6;
        g.range = 30f;
        g.automatic = false;
    }
}
}
