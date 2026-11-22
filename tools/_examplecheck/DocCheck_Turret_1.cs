namespace DocCheck_Turret
{
using Elintria;
using Elintria.Standard;

public class TurretPower : ElintriaScript
{
    public System.Collections.Generic.List<Turret> turrets = new System.Collections.Generic.List<Turret>();
    public void PowerCut() { foreach (Turret t in turrets) t.Deactivate(); }
    public void PowerBack() { foreach (Turret t in turrets) t.Activate(); }
}
}
