namespace DocCheck_LockOnTarget
{
using Elintria;
using Elintria.Standard;

public class WeakPoint : ElintriaScript
{
    public override void Start()
    {
        GameObject core = new GameObject("Weak Point");
        core.transform.SetParent(transform, false);
        core.transform.localPosition = new Vector3(0f, 3f, 0.8f);
        core.AddComponent<LockOnTarget>().aimHeight = 0f;
    }
}
}
