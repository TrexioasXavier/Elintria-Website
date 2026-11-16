namespace DocCheck_Cell
{
using System.Collections.Generic;
using Elintria.Procedural;

public static class CellNeighbours
{
    public static IEnumerable<Cell> Of(Cell c)
    {
        yield return new Cell(c.x + 1, c.y);
        yield return new Cell(c.x - 1, c.y);
        yield return new Cell(c.x, c.y + 1);
        yield return new Cell(c.x, c.y - 1);
    }
}
}
