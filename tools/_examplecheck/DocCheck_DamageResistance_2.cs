namespace DocCheck_DamageResistance
{
using Elintria;
using Elintria.Standard;

public class DamagePreview : ElintriaScript
{
    public DamageResistance target;

    public override void Update()
    {
        if (target != null) ScreenGUI.Text(20f, 180f, "Fire hit: " + target.Modify(20f, DamageType.Fire));
    }
}
}
