namespace DocCheck_WallRunAbility
{
using Elintria;
using Elintria.Standard;

public class WallRunStat : ElintriaScript
{
    private bool m_was;

    public override void Update()
    {
        bool now = GetComponent<WallRunAbility>().running;
        if (now && !m_was) StdVars.Add("wall_runs", 1);
        m_was = now;
    }
}
}
