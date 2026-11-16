namespace DocCheck_Grid
{
using Elintria;
using Elintria.Procedural;

public class CheckReachable : ElintriaScript
{
    public override void Start()
    {
        Grid caves = Dungeon.Caves(60, 40, 5);
        Cell start = new Cell(30, 20);
        for (int x = 0; x < caves.Width; x++) if (Grid.Walkable(caves[x, 20])) { start = new Cell(x, 20); break; }
        var reachable = caves.Reachable(start);
        Debug.Log(reachable.Count + " of " + caves.Count(Grid.Floor) + " floor cells reachable");
    }
}
}
