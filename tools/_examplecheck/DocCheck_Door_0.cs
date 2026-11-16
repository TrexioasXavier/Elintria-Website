namespace DocCheck_Door
{
using Elintria;
using Elintria.Standard;

public class LeverDoorSetup : ElintriaScript
{
    public GameObject door;

    public override void Start()
    {
        Lever lever = GetComponent<Lever>();
        lever.onOnTargets.Add(door);
        lever.onOnMessage = "Open";
        lever.onOffTargets.Add(door);
        lever.onOffMessage = "Close";
    }
}
}
