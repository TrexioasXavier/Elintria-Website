namespace DocCheck_SlideAbility
{
using Elintria;
using Elintria.Standard;

public class SlideNoise : ElintriaScript
{
    private bool m_was;

    public override void Update()
    {
        bool now = GetComponent<SlideAbility>().sliding;
        if (now && !m_was) StdNoise.Make(transform.position, 8f, gameObject);
        m_was = now;
    }
}
}
