namespace DocCheck_Turret
{
using Elintria;
using Elintria.Standard;

public class TurretHack : Interactable
{
    public override void Interact(Interactor who)
    {
        Turret t = GetComponent<Turret>();
        t.targetTag = "Enemy";
        t.laserColor = new Color(0.2f, 0.6f, 1f, 0.6f);
        StdToast.Show("Turret hacked");
    }
}
}
