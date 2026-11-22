namespace DocCheck_TdGame
{
using Elintria;
using Elintria.Standard;

public class Interest : ElintriaScript
{
    private int m_wave;

    public override void Update()
    {
        TdGame g = TdGame.current;
        if (g == null || g.wave == m_wave) return;
        m_wave = g.wave;
        int bonus = g.money / 10;
        g.money += bonus;
        StdToast.Show("Interest +" + bonus);
    }
}
}
