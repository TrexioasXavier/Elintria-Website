namespace DocCheck_LookAtIK
{
using Elintria;
using Elintria.Standard;

public class WatchPlayer : ElintriaScript
{
    public GameObject player;
    public float range = 6f;

    public override void Update()
    {
        LookAtIK ik = GetComponent<LookAtIK>();
        bool near = Vector3.Distance(player.transform.position, transform.position) < range;
        ik.target = player;
        ik.weight = Mathf.MoveTowards(ik.weight, near ? 1f : 0f, Time.deltaTime * 2f);
    }
}
}
