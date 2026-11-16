namespace DocCheck_LockOnTarget
{
using Elintria;
using Elintria.Standard;

public class BossAim : ElintriaScript
{
    public override void Start()
    {
        LockOnTarget t = GetComponent<LockOnTarget>();
        if (t != null) t.aimHeight = 4f;   // aim at the head of a 5 m giant
    }
}
}
