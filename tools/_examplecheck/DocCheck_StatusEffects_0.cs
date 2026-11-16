namespace DocCheck_StatusEffects
{
using Elintria;
using Elintria.Standard;

public class Antidote : ElintriaScript
{
    public void OnUseItem(string id)
    {
        StatusEffects fx = GetComponent<StatusEffects>();
        if (id == "antidote" && fx != null) fx.poisonedUntil = 0f;
    }
}
}
