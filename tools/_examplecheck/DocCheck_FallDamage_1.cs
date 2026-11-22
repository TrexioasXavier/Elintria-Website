namespace DocCheck_FallDamage
{
using Elintria;
using Elintria.Standard;

public class LandingSound : ElintriaScript
{
    public void Landed(float damage)
    {
        StdToast.Show(damage > 30f ? "Ouch!" : "Oof", 1f);
    }
}
}
