namespace DocCheck_QueryTriggerInteraction
{
using Elintria;

public class SolidOnly : ElintriaScript
{
    public override void Start()
    {
        int solid = 0;
        foreach (Collider c in Physics.OverlapSphere(transform.position, 5f))
            if (!c.isTrigger) solid++;
        Debug.Log(solid + " solid colliders nearby");
    }
}
}
