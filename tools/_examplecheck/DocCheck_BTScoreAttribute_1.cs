namespace DocCheck_BTScoreAttribute
{
using Elintria;
using Elintria.AI;

public class DistanceScore : ElintriaScript
{
    public GameObject enemy;
    [BTScore("Enemy Distance")] float EnemyDistance() => enemy == null ? 999f : Vector3.Distance(enemy.transform.position, transform.position);
}
}
