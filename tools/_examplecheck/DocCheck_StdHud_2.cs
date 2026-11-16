namespace DocCheck_StdHud
{
using Elintria;
using Elintria.Standard;

public class HudOff : ElintriaScript
{
    public bool cutscene;

    public override void Update() { GetComponent<StdHud>().enabled = !cutscene; }
}
}
