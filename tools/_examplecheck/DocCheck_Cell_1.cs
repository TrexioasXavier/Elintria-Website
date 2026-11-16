namespace DocCheck_Cell
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class CellVisits : ElintriaScript
{
    readonly Dictionary<Cell, int> visits = new Dictionary<Cell, int>();

    public override void Update()
    {
        var c = new Cell(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
        visits[c] = visits.TryGetValue(c, out int n) ? n + 1 : 1;
    }
}
}
