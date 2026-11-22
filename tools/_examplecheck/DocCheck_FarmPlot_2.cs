namespace DocCheck_FarmPlot
{
using Elintria;
using Elintria.Standard;

public class Crows : ElintriaScript
{
    private bool m_wasDay = true;

    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        if (d == null || d.isDay == m_wasDay) return;
        m_wasDay = d.isDay;
        if (d.isDay) return;
        foreach (FarmPlot p in FindObjectsOfType<FarmPlot>())
            if (p.ripe && Random.value < 0.25f) { p.Clear(); StdToast.Show("Crows ate a crop!"); }
    }
}
}
