namespace DocCheck_Pushable
{
using Elintria;
using Elintria.Standard;

public class ScrapeSound : ElintriaScript
{
    private Vector3 m_last;

    public override void Update()
    {
        if ((transform.position - m_last).sqrMagnitude > 1e-6f) StdNoise.Make(transform.position, 6f, gameObject);
        m_last = transform.position;
    }
}
}
