namespace DocCheck_LevelExit
{
using Elintria;
using Elintria.Standard;

public class ExitAfterObjective : ElintriaScript
{
    public override void Start()
    {
        LevelExit e = GetComponent<LevelExit>();
        e.requires = "quest:escape:done";
        e.lockedMessage = "Find the fuses first";
    }
}
}
