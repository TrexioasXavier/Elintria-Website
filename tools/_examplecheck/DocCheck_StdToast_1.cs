namespace DocCheck_StdToast
{
using Elintria;
using Elintria.Standard;

public class LowAmmoWarning : ElintriaScript
{
    private bool m_said;

    public override void Update()
    {
        Weapon w = GetComponent<Weapon>();
        if (w == null) return;
        if (w.ammo <= 3 && !m_said) { StdToast.Show("Low ammo", 2f, new Color(1f, 0.3f, 0.3f, 1f)); m_said = true; }
        if (w.ammo > 3) m_said = false;
    }
}
}
