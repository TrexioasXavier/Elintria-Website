namespace DocCheck_LevelGenSettings
{
using Elintria;

public class TowerLevel : ElintriaScript
{
    public override void Start()
    {
        var s = new LevelGenSettings("bright office")
            .Seed(77).Floors(3).Rooms(10)
            .Set("vertical", true)      // ladders, lifts and an atrium
            .Set("lightPreset", 5);     // Daylight
        GeneratedLevel level = LevelGenerator.Generate(s);
        if (level != null) Debug.Log("Floors: " + level.floors);
    }
}
}
