namespace DocCheck_StdLine
{
using Elintria;
using Elintria.Standard;

public class PathLine : ElintriaScript
{
    public GameObject target;
    private StdLine m_line;

    public override void LateUpdate()
    {
        if (target == null) return;
        if (m_line == null) m_line = new StdLine(new Color(0.3f, 1f, 0.5f, 1f), true);
        m_line.visible = Input.GetKey(KeyCode.Tab);
        if (m_line.visible) m_line.Set(transform.position + Vector3.up * 0.1f, target.transform.position + Vector3.up * 0.1f, 0.05f);
    }

    public override void OnDestroy() { if (m_line != null) m_line.Destroy(); }
}
}
