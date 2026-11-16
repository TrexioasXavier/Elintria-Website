namespace DocCheck_Companion
{
using Elintria;
using Elintria.Standard;

public class MedicCompanion : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || Time.time < m_next) return;
        Health h = p.GetComponent<Health>();
        if (h.health < h.maxHealth * 0.5f && Vector3.Distance(p.transform.position, transform.position) < 4f)
        {
            h.Heal(25f);
            m_next = Time.time + 20f;
            StdToast.Show("Rook patches you up");
        }
    }
}
}
