namespace DocCheck_Dungeon
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class MazePuzzle : ElintriaScript
{
    public override void Start()
    {
        Grid maze = Dungeon.Maze(12, 12, Random.Range(0, 100000));
        ProcBuild.BuildGrid(maze, 1.5f, transform.position, new Dictionary<int, string> { { Grid.Wall, "HedgeBlock" } }, transform);
    }
}
}
