namespace DocCheck_TdBuildSpot
{
using Elintria;
using Elintria.Standard;

public class HillSpot : ElintriaScript
{
    public override void Update()
    {
        TdBuildSpot s = GetComponent<TdBuildSpot>();
        if (s.occupied && s.tower.range < 12f) s.tower.range = 12f;   // high ground sees further
    }
}
}
