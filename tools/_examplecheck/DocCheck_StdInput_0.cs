namespace DocCheck_StdInput
{
using Elintria;
using Elintria.Standard;

public class AiWalker : ElintriaScript
{
    public GameObject goal;

    public override void Update()
    {
        ThirdPersonController tpc = GetComponent<ThirdPersonController>();
        tpc.input.overridden = true;
        Vector3 to = goal.transform.position - transform.position;
        to.y = 0f;
        tpc.input.move = to.magnitude > 1f ? new Vector2(0f, 1f) : Vector2.zero;   // walk forward
        tpc.yaw = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;                          // face the goal
        tpc.input.jump = Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, 1f);   // hop over low things
    }
}
}
