namespace DocCheck_Throwable
{
using Elintria;
using Elintria.Standard;

public class Landmine : ElintriaScript
{
    private bool m_done;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (m_done) return;
        if (p == null || Vector3.Distance(p.transform.position, transform.position) > 1f) return;
        m_done = true;
        Throwable t = gameObject.AddComponent<Throwable>();
        t.damage = 120f;
        t.radius = 4f;
        t.GoOff(transform.position);
    }
}
}
