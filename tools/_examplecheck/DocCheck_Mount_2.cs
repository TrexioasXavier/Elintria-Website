namespace DocCheck_Mount
{
using Elintria;
using Elintria.Standard;

public class Skittish : ElintriaScript
{
    public void OnDamaged(float amount)
    {
        Mount m = GetComponent<Mount>();
        if (amount > 10f && m.rider != null) m.GetOff();
    }
}
}
