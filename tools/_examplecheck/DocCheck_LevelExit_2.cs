namespace DocCheck_LevelExit
{
using Elintria;
using Elintria.Standard;

public class ExitSave : ElintriaScript
{
    public override void Start() { GetComponent<LevelExit>().saveSlot = 0; }
}
}
