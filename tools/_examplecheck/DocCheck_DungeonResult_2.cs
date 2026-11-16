namespace DocCheck_DungeonResult
{
using Elintria;
using Elintria.Procedural;

public class DungeonPrint : ElintriaScript
{
    public override void Start()
    {
        DungeonResult d = Dungeon.Rooms(40, 24, 8, 6);
        Debug.Log(d.rooms.Count + " rooms, " + d.doors.Count + " doors\n" + d.grid);
    }
}
}
