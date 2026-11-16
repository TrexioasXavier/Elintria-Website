namespace DocCheck_Equipment
{
using Elintria;
using Elintria.Standard;

public class SetBonus : ElintriaScript
{
    public override void Update()
    {
        Equipment e = GetComponent<Equipment>();
        bool set = e.head == "knight_helm" && e.body == "knight_plate";
        GetComponent<Health>().maxHealth = set ? 150f : 100f;
    }
}
}
