namespace DocCheck_StdNoise
{
using Elintria;
using Elintria.Standard;

public class CreakyFloor : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || Time.time < m_next) return;
        if (Vector3.Distance(p.transform.position, transform.position) < 1.5f) { StdNoise.Make(transform.position, 10f, p); m_next = Time.time + 1f; }
    }
}
}
