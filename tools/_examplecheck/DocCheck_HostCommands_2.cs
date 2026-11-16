namespace DocCheck_HostCommands
{
using Elintria;

public class TestProbe : ElintriaScript
{
    public override void Update()
    {
        string c = HostCommands.Take("probe.where");
        if (c != null) HostCommands.Reply("player at " + GameObject.FindWithTag("Player").transform.position);
    }
}
}
