namespace DocCheck_NetRole
{
using Elintria;

public class RoleLogic : ElintriaScript
{
    public override void Update()
    {
        if (NetworkSession.Role == NetRole.Server) { /* authoritative work */ }
        else if (NetworkSession.Role == NetRole.Client) { /* send input */ }
    }
}
}
