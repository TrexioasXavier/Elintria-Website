namespace DocCheck_TdBuildSpot
{
using Elintria;
using Elintria.Standard;

public class LockedSpot : ElintriaScript
{
    public int unlockWave = 3;

    public override void Update()
    {
        TdGame g = TdGame.current;
        GetComponent<TdBuildSpot>().enabled = g != null && g.wave >= unlockWave;
    }
}
}
