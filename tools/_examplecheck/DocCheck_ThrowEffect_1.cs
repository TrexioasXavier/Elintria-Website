namespace DocCheck_ThrowEffect
{
using Elintria;
using Elintria.Standard;

public class FireTrap : ElintriaScript
{
    public float every = 6f;
    private float m_next;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + every;
        ThrowEffect.Make(2, transform.position, 2.5f, 3f, 12f, gameObject);
    }
}
}
