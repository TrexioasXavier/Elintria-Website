namespace DocCheck_StdLine
{
using Elintria;
using Elintria.Standard;

public class Tether : ElintriaScript
{
    public GameObject other;
    private StdLine m_rope;

    public override void LateUpdate()
    {
        if (other == null) return;
        if (m_rope == null) m_rope = new StdLine(new Color(0.3f, 0.25f, 0.2f, 1f));
        m_rope.Set(transform.position, other.transform.position, 0.04f);
    }

    public override void OnDestroy() { if (m_rope != null) m_rope.Destroy(); }
}
}
