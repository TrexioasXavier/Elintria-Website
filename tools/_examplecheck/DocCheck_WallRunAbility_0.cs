namespace DocCheck_WallRunAbility
{
using Elintria;
using Elintria.Standard;

public class WallRunUpgrade : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id == "grip_gloves") GetComponent<WallRunAbility>().maxTime = 2.5f;
    }
}
}
