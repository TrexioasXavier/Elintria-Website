namespace DocCheck_Glider
{
using Elintria;
using Elintria.Standard;

public class GliderUnlock : ElintriaScript
{
    public override void Start() { GetComponent<Glider>().enabled = false; }

    public void OnInventoryChanged()
    {
        if (GetComponent<Inventory>().Has("glider")) GetComponent<Glider>().enabled = true;
    }
}
}
