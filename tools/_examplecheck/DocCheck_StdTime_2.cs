namespace DocCheck_StdTime
{
using Elintria;
using Elintria.Standard;

public class FrameTimer : ElintriaScript
{
    private float m_last;

    public override void Update()
    {
        float now = StdTime.real;
        ScreenGUI.Text(20f, 40f, ((now - m_last) * 1000f).ToString("0.0") + " ms");
        m_last = now;
    }
}
}
