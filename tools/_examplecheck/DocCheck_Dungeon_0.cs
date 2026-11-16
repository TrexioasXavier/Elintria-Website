namespace DocCheck_Dungeon
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class RoomDungeon : ElintriaScript
{
    public int seed = 1;

    public override void Start()
    {
        DungeonResult d = Dungeon.Rooms(64, 64, seed, 12, 5, 11, 0.2f);
        var templates = new Dictionary<int, string> { { Grid.Floor, "FloorTile" }, { Grid.Corridor, "FloorTile" }, { Grid.Wall, "WallBlock" }, { Grid.Door, "DoorFrame" } };
        ProcBuild.BuildGrid(d.grid, 2f, Vector3.zero, templates, transform);
    }
}
}
