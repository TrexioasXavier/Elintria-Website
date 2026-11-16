namespace DocCheck_Grid
{
using Elintria;
using Elintria.Procedural;

public class ArenaGrid : ElintriaScript
{
    public override void Start()
    {
        var g = new Grid(20, 20);
        for (int y = 4; y < 16; y++) for (int x = 4; x < 16; x++) g[x, y] = Grid.Floor;
        g.AddWalls();
        Debug.Log(g.ToString());   // an ASCII picture of the grid
    }
}
}
