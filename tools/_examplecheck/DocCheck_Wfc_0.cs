namespace DocCheck_Wfc
{
using Elintria;
using Elintria.Procedural;

public class RoadTiles : ElintriaScript
{
    public override void Start()
    {
        var wfc = new Wfc()
            .AddTile("grass", "g", "g", "g", "g", 4f)
            .AddTile("road_straight", "r", "g", "r", "g", 1f, true)
            .AddTile("road_corner", "r", "r", "g", "g", 0.5f, true)
            .AddTile("road_cross", "r", "r", "r", "r", 0.2f);
        int[,] map = wfc.Run(16, 16, 42);
        if (map == null) { Debug.LogWarning("No layout found"); return; }
        for (int y = 0; y < 16; y++)
            for (int x = 0; x < 16; x++)
                if (wfc.TileName(map[x, y]).StartsWith("road")) Debug.DrawRay(new Vector3(x, 0f, y), Vector3.up, Color.gray, 10f);
    }
}
}
