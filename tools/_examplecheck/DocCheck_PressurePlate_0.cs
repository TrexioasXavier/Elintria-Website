namespace DocCheck_PressurePlate
{
using Elintria;
using Elintria.Standard;

public class PlateDoorSetup : ElintriaScript
{
    public GameObject door;

    public override void Start()
    {
        PressurePlate p = GetComponent<PressurePlate>();
        p.onPressedTargets.Add(door);  p.onPressedMessage = "Open";
        p.onReleasedTargets.Add(door); p.onReleasedMessage = "Close";
    }
}
}
