namespace DocCheck_LevelGenerator
{
using System.Collections.Generic;
using Elintria;

public class RawManifest : ElintriaScript
{
    public override void Start()
    {
        if (!LevelGenerator.available) return;
        string json = LevelGenerator.BuildManifest("cave", "seed=4;style=2", "", -1);
        if (json == null) return;
        var root = (Dictionary<string, object>)Json.Parse(json);
        Debug.Log("Rooms: " + Json.Int(root, "rooms") + ", meshes: " + Json.List(root, "meshes").Count);
    }
}
}
