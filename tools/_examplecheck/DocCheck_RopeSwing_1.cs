namespace DocCheck_RopeSwing
{
using Elintria;
using Elintria.Standard;

public class WeakRope : ElintriaScript
{
    public float holdSeconds = 3f;
    private float m_since = -1f;

    public override void Update()
    {
        RopeSwing r = GetComponent<RopeSwing>();
        if (!r.swinging) { m_since = -1f; return; }
        if (m_since < 0f) m_since = Time.time;
        if (Time.time - m_since > holdSeconds) { r.Release(); StdToast.Show("The rope snapped!"); }
    }
}
}
