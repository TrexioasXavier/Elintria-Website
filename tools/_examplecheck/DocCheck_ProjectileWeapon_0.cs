namespace DocCheck_ProjectileWeapon
{
using Elintria;
using Elintria.Standard;

public class RocketLauncherSetup : ElintriaScript
{
    public override void Start()
    {
        ProjectileWeapon w = GetComponent<ProjectileWeapon>();
        w.projectileSpeed = 25f;
        w.projectileGravity = 0f;
        w.explosionRadius = 4f;
        w.damage = 100f;
        w.magazineSize = 1;
        w.reloadTime = 2f;
    }
}
}
