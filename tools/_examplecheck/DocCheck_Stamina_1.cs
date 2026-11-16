namespace DocCheck_Stamina
{
using Elintria;
using Elintria.Standard;

public class HeavyArmour : ElintriaScript
{
    public override void Update()
    {
        Equipment e = GetComponent<Equipment>();
        GetComponent<Stamina>().sprintCostPerSecond = e != null && e.body == "plate_armour" ? 25f : 15f;
    }
}
}
