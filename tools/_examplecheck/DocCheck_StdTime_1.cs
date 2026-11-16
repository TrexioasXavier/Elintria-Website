namespace DocCheck_StdTime
{
using Elintria;
using Elintria.Standard;

public class SlowMo : ElintriaScript
{
    private float m_until;

    public void Begin(float seconds) { Time.timeScale = 0.3f; m_until = StdTime.real + seconds; }

    public override void Update()
    {
        if (m_until > 0f && StdTime.real >= m_until) { Time.timeScale = 1f; m_until = 0f; }
    }
}
}
