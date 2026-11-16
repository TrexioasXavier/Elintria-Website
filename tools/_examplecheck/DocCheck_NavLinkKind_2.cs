namespace DocCheck_NavLinkKind
{
using Elintria;

public class DoorWait : ElintriaScript
{
    public override void Update()
    {
        NavAgent a = NavAgent.Of(gameObject);
        if (a.isWaitingForDoor) Debug.Log(name + " is waiting at a door");
        else if (a.currentLinkKind == NavLinkKind.Door) Debug.Log(name + " is going through a door (" + Mathf.RoundToInt(a.linkProgress * 100f) + "%)");
    }
}
}
