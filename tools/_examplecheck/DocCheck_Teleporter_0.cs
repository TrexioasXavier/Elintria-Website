namespace DocCheck_Teleporter
{
using Elintria;
using Elintria.Standard;

public class OneWay : ElintriaScript
{
    public Teleporter backPad;

    public override void Start() { if (backPad != null) backPad.enabled = false; }
}
}
