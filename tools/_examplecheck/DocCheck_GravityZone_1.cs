namespace DocCheck_GravityZone
{
using Elintria;
using Elintria.Standard;

public class MoonBoots : ElintriaScript
{
    public float seconds = 10f;
    private GameObject m_zone;

    public void Use()
    {
        m_zone = GameObject.Create("Moon Boots Zone");
        GravityZone z = m_zone.AddComponent<GravityZone>();
        z.size = new Vector3(3f, 4f, 3f);
        z.gravityScale = 0.3f;
        Destroy(m_zone, seconds);
    }

    public override void Update()
    {
        if (m_zone != null && m_zone.IsAlive) m_zone.transform.position = transform.position + Vector3.up;
    }
}
}
