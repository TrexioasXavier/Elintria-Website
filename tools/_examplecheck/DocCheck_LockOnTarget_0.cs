namespace DocCheck_LockOnTarget
{
using Elintria;
using Elintria.Standard;

public class MarkTarget : ElintriaScript
{
    public void OnSpawned(GameObject spawner)
    {
        if (GetComponent<LockOnTarget>() == null) gameObject.AddComponent<LockOnTarget>().aimHeight = 1.4f;
    }
}
}
