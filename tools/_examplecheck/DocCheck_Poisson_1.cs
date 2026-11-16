namespace DocCheck_Poisson
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class SpreadSpawns : ElintriaScript
{
    public List<Vector3> Make(int seed)
    {
        var result = new List<Vector3>();
        foreach (Vector2 p in Poisson.Sample(60f, 60f, 15f, seed))
            result.Add(new Vector3(p.x - 30f, 1f, p.y - 30f));
        return result;
    }
}
}
