namespace DocCheck_QueryTriggerInteraction
{
using Elintria;

public class OverlapTriggers : ElintriaScript
{
    public override void Start()
    {
        Collider[] all = Physics.OverlapSphere(transform.position, 5f, -1, QueryTriggerInteraction.Collide);
        Debug.Log(all.Length + " colliders nearby");
    }
}
}
