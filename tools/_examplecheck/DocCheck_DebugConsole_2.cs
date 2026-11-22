namespace DocCheck_DebugConsole
{
using Elintria;
using Elintria.Standard;

public class TestSetup : ElintriaScript
{
    public override void Start()
    {
        DebugConsole.Execute("give wood 50");
        DebugConsole.Execute("money 500");
        DebugConsole.Print("Test kit given");
    }
}
}
