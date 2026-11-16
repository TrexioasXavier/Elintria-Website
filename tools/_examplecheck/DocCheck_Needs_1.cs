namespace DocCheck_Needs
{
using Elintria;
using Elintria.Standard;

public class SurvivalMode : ElintriaScript
{
    public bool survival = false;

    public override void Start()
    {
        Needs n = GetComponent<Needs>();
        n.useHunger = survival;
        n.useThirst = survival;
        n.useTemperature = survival;
    }
}
}
