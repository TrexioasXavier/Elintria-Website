namespace DocCheck_PhysicsGrabber
{
using Elintria;
using Elintria.Standard;

public class ThrownDamage : ElintriaScript
{
    private GameObject m_last;

    public override void Update()
    {
        PhysicsGrabber g = GetComponent<PhysicsGrabber>();
        if (g.held != null) m_last = g.held;
        else if (m_last != null && Input.GetMouseButtonDown(0))
        {
            foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
                if (Vector3.Distance(e.transform.position, m_last.transform.position) < 1.5f) StdDamage.Apply(e.gameObject, 25f, gameObject, e.transform.position, Vector3.up, 4f);
            m_last = null;
        }
    }
}
}
