namespace DocCheck_TdEnemy
{
using Elintria;
using Elintria.Standard;

public class TdHealer : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + 2f;
        foreach (TdEnemy e in TdEnemy.all)
            if (e != null && e.alive && Vector3.Distance(e.transform.position, transform.position) < 4f) e.GetComponent<Health>().Heal(10f);
    }
}
}
