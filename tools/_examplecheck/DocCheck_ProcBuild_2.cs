namespace DocCheck_ProcBuild
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class HedgeMaze : ElintriaScript
{
    public override void Start()
    {
        Grid maze = Dungeon.Maze(10, 10, 5);
        List<GameObject> hedges = ProcBuild.BuildGrid(maze, 2f, transform.position, new Dictionary<int, string> { { Grid.Wall, "Hedge" } }, transform);
        foreach (GameObject h in hedges) h.transform.localScale = new Vector3(2f, 2.5f, 2f);
    }
}
}
