namespace DocCheck_Grid
{
using Elintria;
using Elintria.Procedural;

public class DistanceDifficulty : ElintriaScript
{
    public override void Start()
    {
        DungeonResult d = Dungeon.Rooms(64, 64, 11);
        int[,] dist = d.grid.Distances(d.start);
        foreach (Room r in d.rooms)
        {
            int steps = dist[r.Center.x, r.Center.y];
            Debug.Log("Room at " + r.Center + ": " + (steps < 20 ? "easy" : steps < 50 ? "medium" : "hard"));
        }
    }
}
}
