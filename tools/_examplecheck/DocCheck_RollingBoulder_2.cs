namespace DocCheck_RollingBoulder
{
using Elintria;
using Elintria.Standard;

public class RunWarning : ElintriaScript
{
    private bool m_said;

    public override void Update()
    {
        if (GetComponent<RollingBoulder>().rolling && !m_said) { StdToast.Show("RUN!", 2f, new Color(1f, 0.3f, 0.2f, 1f)); m_said = true; }
    }
}
}
