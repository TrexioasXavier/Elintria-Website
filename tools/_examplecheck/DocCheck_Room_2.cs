namespace DocCheck_Room
{
using Elintria;
using Elintria.Procedural;

public static class RoomOfPlayer
{
    public static int Index(DungeonResult d, Vector3 playerPos, float cellSize)
    {
        var c = new Cell((int)(playerPos.x / cellSize), (int)(playerPos.z / cellSize));
        return d.rooms.FindIndex(r => r.Contains(c));
    }
}
}
