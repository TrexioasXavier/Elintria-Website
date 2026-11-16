namespace DocCheck_Needs
{
using Elintria;
using Elintria.Standard;

public class HungerWarning : ElintriaScript
{
    private bool m_warned;

    public override void Update()
    {
        float h = GetComponent<Needs>().hunger;
        if (h < 20f && !m_warned) { StdToast.Show("You are hungry"); m_warned = true; }
        if (h > 40f) m_warned = false;
    }
}
}
