namespace DocCheck_Dungeon
{
using Elintria;
using Elintria.Procedural;

public class MiningCaves : ElintriaScript
{
    public override void Start()
    {
        Grid g = Dungeon.Caves(80, 80, 21, 0.47f, 5);
        int ore = 0;
        var rng = new Rng(21);
        for (int y = 0; y < g.Height; y++)
            for (int x = 0; x < g.Width; x++)
                if (g[x, y] == Grid.Wall && rng.Chance(0.02f)) ore++;
        Debug.Log("Cave with " + g.Count(Grid.Floor) + " floor cells and " + ore + " ore veins");
    }
}
}
