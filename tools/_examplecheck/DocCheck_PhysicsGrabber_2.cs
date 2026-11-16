namespace DocCheck_PhysicsGrabber
{
using Elintria;
using Elintria.Standard;

public class HoldPrompt : ElintriaScript
{
    public override void Update()
    {
        if (GetComponent<PhysicsGrabber>().held != null) ScreenGUI.Text(20f, 240f, "Left click: throw   G: drop   Wheel: turn");
    }
}
}
