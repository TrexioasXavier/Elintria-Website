namespace DocCheck_Throwable
{
using Elintria;
using Elintria.Standard;

public class GrenadeLobber : ElintriaScript
{
    public float every = 5f;
    private float m_next;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || Time.time < m_next) return;
        m_next = Time.time + every;
        Vector3 to = p.transform.position - transform.position;
        GameObject g = StdBoxVisual.Make("Enemy Grenade", transform.position + Vector3.up * 1.6f, Vector3.one * 0.2f, new Color(0.3f, 0.35f, 0.2f, 1f));
        Throwable t = g.AddComponent<Throwable>();
        t.owner = gameObject;
        t.velocity = new Vector3(to.x, 0f, to.z) * 0.6f + Vector3.up * 7f;
    }
}
}
