namespace DocCheck_BTActionAttribute
{
using Elintria;
using Elintria.AI;

public class ContextAction : ElintriaScript
{
    [BTAction] BTStatus Patrol(BTContext c)
    {
        transform.Rotate(0f, 45f * c.deltaTime, 0f);
        return BTStatus.Running;
    }
}
}
