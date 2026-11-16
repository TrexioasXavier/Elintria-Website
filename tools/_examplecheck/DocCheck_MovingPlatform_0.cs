namespace DocCheck_MovingPlatform
{
using Elintria;
using Elintria.Standard;

public class PlatePlatformSetup : ElintriaScript
{
    public GameObject platform;

    public override void Start()
    {
        platform.GetComponent<MovingPlatform>().waitForActivate = true;
        PressurePlate plate = GetComponent<PressurePlate>();
        plate.onPressedTargets.Add(platform);
        plate.onPressedMessage = "Activate";
    }
}
}
