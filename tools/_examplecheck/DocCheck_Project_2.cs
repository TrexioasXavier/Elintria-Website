namespace DocCheck_Project
{
using Elintria;

public class ConfigPath : ElintriaScript
{
    public string configFile = "Settings/game.ini";   // or C:\Games\game.ini

    public override void Start()
    {
        string full = Project.Resolve(configFile);
        Debug.Log(System.IO.File.Exists(full) ? "Config at " + full : "No config at " + full);
    }
}
}
