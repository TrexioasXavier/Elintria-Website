namespace DocCheck_FallDamage
{
using Elintria;
using Elintria.Standard;

public class ToughLegs : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "leg_braces") return;
        FallDamage f = GetComponent<FallDamage>();
        f.safeHeight = 8f;
        f.lethalHeight = 30f;
    }
}
}
