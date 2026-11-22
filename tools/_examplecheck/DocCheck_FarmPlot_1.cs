namespace DocCheck_FarmPlot
{
using Elintria;
using Elintria.Standard;

public class HarvestCounter : ElintriaScript
{
    public void Harvested(int count) { StdVars.Add("crops_harvested", count); }
}
}
