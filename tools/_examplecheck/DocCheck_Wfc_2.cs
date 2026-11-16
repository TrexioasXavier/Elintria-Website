namespace DocCheck_Wfc
{
using Elintria;
using Elintria.Procedural;

public class FixedEntrance : ElintriaScript
{
    public override void Start()
    {
        var wfc = new Wfc()
            .AddTile("wall", "w", "w", "w", "w")
            .AddTile("hall", "h", "h", "h", "h")
            .AddTile("door", "h", "w", "w", "w", 0.3f, true);
        wfc.Set(0, 8, "door");     // the entrance always here
        int[,] layout = wfc.Run(16, 16, 3, 20);
        wfc.ClearFixed();
        Debug.Log(layout == null ? "failed" : "entrance tile: " + wfc.TileName(layout[0, 8]));
    }
}
}
