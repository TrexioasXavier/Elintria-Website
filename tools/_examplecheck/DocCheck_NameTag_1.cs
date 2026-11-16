namespace DocCheck_NameTag
{
using Elintria;
using Elintria.Standard;

public class HostileTag : ElintriaScript
{
    public override void Update()
    {
        BasicEnemy e = GetComponent<BasicEnemy>();
        GetComponent<NameTag>().color = e != null && e.state == "Chase" ? new Color(1f, 0.3f, 0.3f, 1f) : Color.white;
    }
}
}
