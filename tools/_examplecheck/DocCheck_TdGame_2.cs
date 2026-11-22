namespace DocCheck_TdGame
{
using Elintria;
using Elintria.Standard;

public class EndlessTd : ElintriaScript
{
    public override void Update()
    {
        TdGame g = TdGame.current;
        if (g != null && g.wave >= g.waveCount - 1) g.waveCount += 5;   // never runs out
    }
}
}
