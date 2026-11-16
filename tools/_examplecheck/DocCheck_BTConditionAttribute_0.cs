namespace DocCheck_BTConditionAttribute
{
using Elintria;
using Elintria.AI;

public class EnemySenses : ElintriaScript
{
    public float health = 100f;
    public GameObject player;

    [BTCondition] bool LowHealth() => health < 25f;
    [BTCondition] bool PlayerNear() => player != null && Vector3.Distance(player.transform.position, transform.position) < 10f;
}
}
