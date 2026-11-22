namespace DocCheck_BoostRing
{
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class RingCourse : ElintriaScript
{
    public List<BoostRing> rings = new List<BoostRing>();
    private float m_start = -1f;

    public override void Update()
    {
        int done = 0;
        foreach (BoostRing r in rings) if (r.passes > 0) ++done;
        if (done > 0 && m_start < 0f) m_start = Time.time;
        if (m_start >= 0f) ScreenGUI.Text(20f, 60f, done + " / " + rings.Count + "   " + (Time.time - m_start).ToString("0.0") + " s", null, Color.white);
    }
}
}
