namespace DocCheck_NavCrowd
{
using Elintria;

public class HordeSettings : ElintriaScript
{
    public override void Start()
    {
        NavCrowd.Configure(maxNeighbors: 6, quality: 1, flowFields: 1, lodDistance: 40f);
    }
}
}
