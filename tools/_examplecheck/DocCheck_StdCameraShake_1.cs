namespace DocCheck_StdCameraShake
{
using Elintria;
using Elintria.Standard;

public class GiantSteps : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + 1.2f;
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null) return;
        float d = Vector3.Distance(p.transform.position, transform.position);
        if (d < 40f) StdCameraShake.Shake(0.2f * (1f - d / 40f), 0.4f);
    }
}
}
