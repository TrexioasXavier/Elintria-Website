namespace DocCheck_Swimmer
{
using Elintria;
using Elintria.Standard;

public class DivingMask : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id == "diving_mask") GetComponent<Swimmer>().oxygenSeconds = 90f;
    }
}
}
