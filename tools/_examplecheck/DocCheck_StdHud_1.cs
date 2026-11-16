namespace DocCheck_StdHud
{
using Elintria;
using Elintria.Standard;

public class NearestEnemyMarker : ElintriaScript
{
    public override void Update()
    {
        BasicEnemy best = null; float d = 30f;
        foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
        {
            float x = Vector3.Distance(e.transform.position, transform.position);
            if (x < d) { d = x; best = e; }
        }
        if (best != null) StdHud.Marker(best.transform.position + Vector3.up * 2f, new Color(1f, 0.3f, 0.3f, 1f), d);
    }
}
}
