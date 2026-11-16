namespace DocCheck_HitscanWeapon
{
using Elintria;
using Elintria.Standard;

public class SniperSetup : ElintriaScript
{
    public GameObject sparks;   // an inactive effect object

    public override void Start()
    {
        HitscanWeapon g = GetComponent<HitscanWeapon>();
        g.damage = 90f;
        g.range = 300f;
        g.spread = 0f;
        g.fireRate = 0.8f;
        g.impactTemplate = sparks;
        g.impactLifetime = 1.5f;
    }
}
}
