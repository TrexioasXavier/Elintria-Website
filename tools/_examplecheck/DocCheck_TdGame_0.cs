namespace DocCheck_TdGame
{
using Elintria;
using Elintria.Standard;

public class FreeTower : ElintriaScript
{
    public TdBuildSpot spot;
    private bool m_done;

    public override void Update()
    {
        TdGame g = TdGame.current;
        if (m_done || g == null) return;
        m_done = true;
        g.money += g.TowerCost(0);
        g.Build(spot, 0);
    }
}
}
