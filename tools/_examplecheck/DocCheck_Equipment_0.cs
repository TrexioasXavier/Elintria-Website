namespace DocCheck_Equipment
{
using Elintria;
using Elintria.Standard;

public class GearList : ElintriaScript
{
    public override void Awake()
    {
        GetComponent<Equipment>().items = "hood:head:armor=1|chainmail:body:armor=12,speed=-0.1|bow:weapon|axe:weapon";
    }
}
}
