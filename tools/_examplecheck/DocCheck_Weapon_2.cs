namespace DocCheck_Weapon
{
using Elintria;
using Elintria.Standard;

public class Accuracy : ElintriaScript
{
    public override void Update()
    {
        Weapon w = GetComponent<HitscanWeapon>();
        if (w == null || w.shotsFired == 0) return;
        ScreenGUI.Text(20, 60, "Accuracy " + Mathf.RoundToInt(100f * w.hits / w.shotsFired) + "%  " + (w.reloading ? "RELOADING" : w.ammo + "/" + w.reserveAmmo));
    }
}
}
