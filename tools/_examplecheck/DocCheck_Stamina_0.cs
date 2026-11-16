namespace DocCheck_Stamina
{
using Elintria;
using Elintria.Standard;

public class EnergyDrink : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "energy_drink") return;
        Stamina s = GetComponent<Stamina>();
        s.stamina = s.maxStamina;
        s.regenPerSecond = 40f;
    }
}
}
