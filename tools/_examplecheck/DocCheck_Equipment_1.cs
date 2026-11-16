namespace DocCheck_Equipment
{
using Elintria;
using Elintria.Standard;

public class StartingGear : ElintriaScript
{
    public override void Start()
    {
        GetComponent<Inventory>().Add("rifle", 1);
        GetComponent<Equipment>().Equip("rifle");
    }
}
}
