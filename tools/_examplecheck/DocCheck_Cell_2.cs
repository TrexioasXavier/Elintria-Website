namespace DocCheck_Cell
{
using Elintria;
using Elintria.Procedural;

public static class CellToWorld
{
    public static Vector3 Center(Cell c, float cellSize) => new Vector3((c.x + 0.5f) * cellSize, 0f, (c.y + 0.5f) * cellSize);
}
}
