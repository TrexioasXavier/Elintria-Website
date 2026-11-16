namespace DocCheck_ThirdPersonController
{
using Elintria;
using Elintria.Standard;

public class LockOnHud : ElintriaScript
{
    public override void Update()
    {
        var tpc = GetComponent<ThirdPersonController>();
        if (tpc.lockTarget != null)
            ScreenGUI.Label(new Rect(0, 40, ScreenGUI.screenWidth, 30), "Locked on: " + tpc.lockTarget.name, null, Color.red, TextAnchor.MiddleCenter);
        else if (tpc.FindLockTarget() != null)
            ScreenGUI.Label(new Rect(0, 40, ScreenGUI.screenWidth, 30), "[Q] Lock on", null, Color.white, TextAnchor.MiddleCenter);
    }
}
}
