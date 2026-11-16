namespace DocCheck_Climber
{
using Elintria;
using Elintria.Standard;

public class LooseRocks : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        Climber c = GetComponent<Climber>();
        if (!c.climbing || Time.time < m_next) return;
        m_next = Time.time + 5f;
        if (Random.value < 0.3f) c.LetGo(Vector3.zero);
    }
}
}
