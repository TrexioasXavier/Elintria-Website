namespace DocCheck_DamageNumbers
{
using Elintria;
using Elintria.Standard;

public class NumbersSetting : ElintriaScript
{
    public bool show = true;

    public override void Update()
    {
        foreach (DamageNumbers d in FindObjectsOfType<DamageNumbers>()) d.enabled = show;
    }
}
}
