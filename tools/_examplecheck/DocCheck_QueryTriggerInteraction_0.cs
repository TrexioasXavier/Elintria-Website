namespace DocCheck_QueryTriggerInteraction
{
using Elintria;

public class IgnoreTriggers : ElintriaScript
{
    public override void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 50f, -1, QueryTriggerInteraction.Ignore);
    }
}
}
