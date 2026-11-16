namespace DocCheck_LevelMarker
{
using Elintria;

public class KeyPlacer : ElintriaScript
{
    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("dungeon", "seed=2;mission=1");
        if (level == null) return;
        foreach (LevelMarker m in level.Markers("Key"))
            Debug.Log("Key \"" + m.item + "\" in room " + m.room + " on floor " + (m.floor + 1));
    }
}
}
