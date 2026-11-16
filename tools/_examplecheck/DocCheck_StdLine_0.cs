namespace DocCheck_StdLine
{
using Elintria;
using Elintria.Standard;

public class LaserSight : ElintriaScript
{
    private StdLine m_line;

    public override void LateUpdate()
    {
        if (m_line == null) m_line = new StdLine(new Color(1f, 0.1f, 0.1f, 1f), true);
        Vector3 eye, dir; float fov;
        StdScreen.View(out eye, out dir, out fov);
        m_line.Set(transform.position + Vector3.up * 1.4f, eye + dir * 30f, 0.01f);
    }

    public override void OnDestroy() { if (m_line != null) m_line.Destroy(); }
}
}
