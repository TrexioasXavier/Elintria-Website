namespace DocCheck_Teleporter
{
using Elintria;
using Elintria.Standard;

public class EnemyPad : ElintriaScript
{
    public override void Update()
    {
        Teleporter t = GetComponent<Teleporter>();
        foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
            if (Vector3.Distance(e.transform.position, transform.position) < t.radius) e.transform.position = t.target.transform.position;
    }
}
}
