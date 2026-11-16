namespace DocCheck_PressurePlate
{
using Elintria;
using Elintria.Standard;

public class TrapPlate : ElintriaScript
{
    public GameObject arrowShooter;   // a ProjectileWeapon

    public void Fire() { arrowShooter.GetComponent<ProjectileWeapon>().TryFire(); }   // set as the onPressed message, targeting this object
}
}
