namespace DocCheck_Throwable
{
using Elintria;
using Elintria.Standard;

public class QuickFuse : ElintriaScript
{
    public override void Update()
    {
        ThrowableUser u = GetComponent<ThrowableUser>();
        if (u.lastThrown == null || !u.lastThrown.IsAlive) return;
        Throwable t = u.lastThrown.GetComponent<Throwable>();
        if (t != null && t.kind == 0) t.fuse = Mathf.Min(t.fuse, 1.2f);
    }
}
}
