namespace DocCheck_StdHud
{
using Elintria;
using Elintria.Standard;

public class MinimalHud : ElintriaScript
{
    public override void Start()
    {
        StdHud h = GetComponent<StdHud>();
        h.showHealth = false;
        h.showStamina = false;
        h.showAmmo = false;
    }
}
}
