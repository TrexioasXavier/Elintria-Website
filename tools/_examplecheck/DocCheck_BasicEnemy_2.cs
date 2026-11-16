namespace DocCheck_BasicEnemy
{
using Elintria;
using Elintria.Standard;

public class Squad : ElintriaScript
{
    public GameObject enemyTemplate;

    public override void Start()
    {
        for (int i = 0; i < 4; ++i)
        {
            GameObject g = Instantiate(enemyTemplate, transform.position + new Vector3(i * 2f, 0f, 0f), Quaternion.identity);
            BasicEnemy e = g.GetComponent<BasicEnemy>();
            e.attackMode = i % 2;
            e.patrolPoints.Add(gameObject);
        }
    }
}
}
