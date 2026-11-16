namespace DocCheck_StatusEffects
{
using Elintria;
using Elintria.Standard;

public class Douse : ElintriaScript
{
    public override void Update()
    {
        StatusEffects fx = GetComponent<StatusEffects>();
        Swimmer s = GetComponent<Swimmer>();
        if (fx != null && s != null && s.swimming) fx.burningUntil = 0f;
    }
}
}
