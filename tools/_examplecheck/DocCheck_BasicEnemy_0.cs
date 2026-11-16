namespace DocCheck_BasicEnemy
{
using Elintria;
using Elintria.Standard;

public class AlarmOnSight : ElintriaScript
{
    public override void Update()
    {
        BasicEnemy me = GetComponent<BasicEnemy>();
        if (me.state != "Chase") return;
        foreach (BasicEnemy other in FindObjectsOfType<BasicEnemy>())
            if (other != me && other.state == "Patrol" && Vector3.Distance(other.transform.position, transform.position) < 20f)
                StdNoise.Make(transform.position, 25f, gameObject);
    }
}
}
