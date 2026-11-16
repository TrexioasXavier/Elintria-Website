namespace DocCheck_Room
{
using System.Linq;
using Elintria;
using Elintria.Procedural;

public class BossRoom : ElintriaScript
{
    public static Room Biggest(DungeonResult d) => d.rooms.OrderByDescending(r => r.w * r.h).First();
}
}
