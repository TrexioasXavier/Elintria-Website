namespace DocCheck_DebugConsole
{
using Elintria;
using Elintria.Standard;

public class ReleaseConsole : ElintriaScript
{
    public bool releaseBuild = true;

    public override void Start()
    {
        DebugConsole c = GetComponent<DebugConsole>();
        c.cheatsAllowed = !releaseBuild;
        c.showFps = !releaseBuild;
    }
}
}
