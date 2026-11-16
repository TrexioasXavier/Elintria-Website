namespace DocCheck_StdPatrol
{
using Elintria;
using Elintria.Standard;

public class Guard : ElintriaScript
{
    public GameObject player;
    public float sight = 8f;

    public override void Update()
    {
        StdPatrol patrol = GetComponent<StdPatrol>();
        bool sees = Vector3.Distance(transform.position, player.transform.position) < sight;
        patrol.patrolling = !sees;
        if (sees) transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 3f * Time.deltaTime);
    }
}
}
