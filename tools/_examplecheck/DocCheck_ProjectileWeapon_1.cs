namespace DocCheck_ProjectileWeapon
{
using Elintria;
using Elintria.Standard;

public class GrenadeLauncherSetup : ElintriaScript
{
    public override void Start()
    {
        ProjectileWeapon w = GetComponent<ProjectileWeapon>();
        w.projectileSpeed = 15f;
        w.projectileGravity = 9.8f;   // arcs
        w.explosionRadius = 5f;
        w.fireRate = 1f;
    }
}
}
