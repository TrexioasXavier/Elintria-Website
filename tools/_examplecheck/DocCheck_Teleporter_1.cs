namespace DocCheck_Teleporter
{
using Elintria;
using Elintria.Standard;

public class PoweredPad : ElintriaScript
{
    public override void Update() { GetComponent<Teleporter>().enabled = StdVars.Is("power", "on"); }
}
}
