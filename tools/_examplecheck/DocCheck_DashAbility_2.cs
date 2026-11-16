namespace DocCheck_DashAbility
{
using Elintria;
using Elintria.Standard;

public class DashUpgrade : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "dash_core") return;
        DashAbility d = GetComponent<DashAbility>();
        d.distance = 9f;
        d.cooldown = 0.5f;
    }
}
}
