namespace DocCheck_TdEnemy
{
using Elintria;
using Elintria.Standard;

public class TdLeader : ElintriaScript
{
    public override void Update()
    {
        TdEnemy lead = null;
        foreach (TdEnemy e in TdEnemy.all) if (e != null && e.alive && (lead == null || e.distance > lead.distance)) lead = e;
        if (lead != null) StdHud.Marker(lead.transform.position + Vector3.up * 1.5f, new Color(1f, 0.3f, 0.3f, 1f), -1f);
    }
}
}
