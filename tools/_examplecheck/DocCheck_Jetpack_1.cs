namespace DocCheck_Jetpack
{
using Elintria;
using Elintria.Standard;

public class NoRefuelZone : ElintriaScript
{
    public float radius = 20f;
    private float m_normal = -1f;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        Jetpack j = p != null ? p.GetComponent<Jetpack>() : null;
        if (j == null) return;
        if (m_normal < 0f) m_normal = j.refuelSeconds;
        j.refuelSeconds = Vector3.Distance(p.transform.position, transform.position) < radius ? 9999f : m_normal;
    }
}
}
