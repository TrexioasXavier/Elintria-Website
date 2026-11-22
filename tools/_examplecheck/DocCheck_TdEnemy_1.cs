namespace DocCheck_TdEnemy
{
using Elintria;
using Elintria.Standard;

public class TdPanic : ElintriaScript
{
    public void OnDamaged(float amount) { GetComponent<TdEnemy>().speed *= 1.05f; }
}
}
