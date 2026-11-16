namespace DocCheck_Room
{
using System.Collections.Generic;
using Elintria.Procedural;

public static class RoomPacker
{
    public static List<Room> Pack(int count, int seed)
    {
        var rng = new Rng(seed);
        var rooms = new List<Room>();
        for (int tries = 0; tries < 500 && rooms.Count < count; tries++)
        {
            var r = new Room(rng.Range(0, 50), rng.Range(0, 50), rng.Range(4, 10), rng.Range(4, 10));
            if (!rooms.Exists(o => o.Overlaps(r, 1))) rooms.Add(r);
        }
        return rooms;
    }
}
}
