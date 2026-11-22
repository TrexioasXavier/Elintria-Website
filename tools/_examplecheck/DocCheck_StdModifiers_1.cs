namespace DocCheck_StdModifiers
{
using Elintria;
using Elintria.Standard;

public class DamageSheet : ElintriaScript
{
    public override void Update()
    {
        float k = StdModifiers.DamageMultiplier(gameObject);
        UIRuntime.SetText("DamageText", "Damage x" + k.ToString("0.00"));
    }
}
}
