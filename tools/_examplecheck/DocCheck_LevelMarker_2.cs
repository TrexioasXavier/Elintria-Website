namespace DocCheck_LevelMarker
{
using System.Collections.Generic;
using Elintria;

public class MarkerCount : ElintriaScript
{
    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("office", "seed=3");
        if (level == null) return;
        var counts = new Dictionary<string, int>();
        foreach (LevelMarker m in level.markers)
            counts[m.kind] = counts.TryGetValue(m.kind, out int n) ? n + 1 : 1;
        foreach (var kv in counts) Debug.Log(kv.Key + ": " + kv.Value);
    }
}
}
