namespace DocCheck_LevelGenSettings
{
using Elintria;

public class SettingsLog : ElintriaScript
{
    public override void Start()
    {
        var s = new LevelGenSettings().Theme("Hospital").Seed(3).Set("secrets", true).Set("secretRooms", 2);
        Debug.Log("Asking for: " + s.Values());   // theme=Hospital;seed=3;secrets=1;secretRooms=2
    }
}
}
