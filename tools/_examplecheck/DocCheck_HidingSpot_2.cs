namespace DocCheck_HidingSpot
{
using Elintria;
using Elintria.Standard;

public class HideCounter : ElintriaScript
{
    private bool m_was;

    public override void Update()
    {
        bool now = GetComponent<HidingSpot>().occupied;
        if (now && !m_was) StdVars.Add("times_hidden", 1);
        m_was = now;
    }
}
}
