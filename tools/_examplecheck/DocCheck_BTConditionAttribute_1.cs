namespace DocCheck_BTConditionAttribute
{
using Elintria;
using Elintria.AI;

public class RenamedCondition : ElintriaScript
{
    public int arrows = 5;
    [BTCondition("Has Arrows")] bool HasAmmo() => arrows > 0;
}
}
