namespace DocCheck_NavMeshRuntime
{
using Elintria;

public class NavDoor : ElintriaScript
{
    public GameObject sideA;
    public GameObject sideB;
    int link;

    public override void Start() { link = NavMeshRuntime.AddLink(sideA.transform.position, sideB.transform.position, NavLinkKind.Door); }

    public void Open() { NavMeshRuntime.SetLinkOpen(link, true); }
    public void Close() { NavMeshRuntime.SetLinkOpen(link, false); }
    public void Lock(bool locked) { NavMeshRuntime.SetLinkEnabled(link, !locked); }
}
}
