namespace DocCheck_CharacterStats
{
using Elintria;
using Elintria.Standard;

public class DiscoveryXp : ElintriaScript
{
    private bool m_found;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (m_found || p == null || Vector3.Distance(p.transform.position, transform.position) > 5f) return;
        m_found = true;
        p.GetComponent<CharacterStats>().AddXp(40);
        StdToast.Show("Discovered: " + gameObject.name);
    }
}
}
