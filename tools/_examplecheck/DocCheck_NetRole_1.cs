namespace DocCheck_NetRole
{
using Elintria;

public class RoleText : ElintriaScript
{
    public override void Update() { ScreenGUI.Text(20, 40, "Role: " + NetworkSession.Role); }
}
}
