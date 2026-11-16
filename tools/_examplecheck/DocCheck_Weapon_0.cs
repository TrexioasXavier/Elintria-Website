namespace DocCheck_Weapon
{
using Elintria;
using Elintria.Standard;

public class AmmoBox : ElintriaScript
{
    public int rounds = 60;

    public override void OnTriggerEnter(Collider other)
    {
        Weapon w = other.gameObject.GetComponentInChildren<HitscanWeapon>();
        if (w == null) return;
        w.reserveAmmo += rounds;
        Destroy(gameObject);
    }
}
}
