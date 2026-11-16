namespace DocCheck_DamageType
{
using Elintria;
using Elintria.Standard;

public class TypeReaction : ElintriaScript
{
    public void OnDamaged(float amount)
    {
        if (StdEvents.currentType == DamageType.Fire) StdToast.Show("Burning!");
    }
}
}
