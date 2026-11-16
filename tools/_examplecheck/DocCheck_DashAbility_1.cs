namespace DocCheck_DashAbility
{
using Elintria;
using Elintria.Standard;

public class DoubleTapDash : ElintriaScript
{
    private float m_last = -1f;

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.W)) return;
        if (Time.time - m_last < 0.25f) GetComponent<DashAbility>().Dash();
        m_last = Time.time;
    }
}
}
