namespace DocCheck_DebugConsole
{
using Elintria;
using Elintria.Standard;

public class SkipLevelCommand : ElintriaScript
{
    public string nextScene = "Level2";

    public override void Start()
    {
        DebugConsole.Register("skip", "skip: goes to the next level", args =>
        {
            SceneManager.LoadScene(nextScene);
            return "Loading " + nextScene;
        }, true);
    }
}
}
