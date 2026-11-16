namespace DocCheck_LevelExit
{
using Elintria;
using Elintria.Standard;

public class NextLevel : ElintriaScript
{
    public override void Start()
    {
        int n = StdVars.GetInt("level_number", 1);
        GetComponent<LevelExit>().sceneName = "Scenes/Level" + (n + 1) + ".scene";
        StdVars.Set("level_number", (n + 1).ToString());
    }
}
}
