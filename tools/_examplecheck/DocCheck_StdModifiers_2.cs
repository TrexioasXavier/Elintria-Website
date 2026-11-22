namespace DocCheck_StdModifiers
{
using Elintria;
using Elintria.Standard;

public class FighterCompare : ElintriaScript
{
    public GameObject a;
    public GameObject b;

    public override void Start()
    {
        float ka = StdModifiers.DamageMultiplier(a), kb = StdModifiers.DamageMultiplier(b);
        Debug.Log((ka >= kb ? a.name : b.name) + " hits harder (" + ka.ToString("0.00") + " vs " + kb.ToString("0.00") + ")");
    }
}
}
