namespace DocCheck_GameWindow
{
using Elintria;

public class LockedLook : ElintriaScript
{
    float yaw;

    public override void Update()
    {
        if (!GameWindow.hasFocus) return;   // don't steal the mouse when alt-tabbed
        GameWindow.SetCursorLocked(true);
        yaw += GameWindow.LockedDelta().x * 0.15f;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    public override void OnDisable() { GameWindow.SetCursorLocked(false); }
}
}
