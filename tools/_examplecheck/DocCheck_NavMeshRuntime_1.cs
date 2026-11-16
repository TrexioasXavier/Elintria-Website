namespace DocCheck_NavMeshRuntime
{
using Elintria;

public class LadderLink : ElintriaScript
{
    public GameObject bottom;
    public GameObject top;
    int link = -1;

    public override void Start()
    {
        link = NavMeshRuntime.AddLink(bottom.transform.position, top.transform.position, NavLinkKind.Climb, true, 3f);
        if (link < 0) Debug.LogWarning("A ladder end is off the nav mesh");
    }

    public override void OnDestroy() { if (link >= 0) NavMeshRuntime.RemoveLink(link); }
}
}
