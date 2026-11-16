namespace DocCheck_NavLinkKind
{
using Elintria;

public class GapJump : ElintriaScript
{
    public GameObject landing;

    public override void Start()
    {
        NavMeshRuntime.AddLink(transform.position, landing.transform.position, NavLinkKind.Jump, false);
    }
}
}
