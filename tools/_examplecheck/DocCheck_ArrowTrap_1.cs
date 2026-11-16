namespace DocCheck_ArrowTrap
{
using Elintria;
using Elintria.Standard;

public class Volley : ElintriaScript
{
    public void Fire()
    {
        foreach (ArrowTrap t in FindObjectsOfType<ArrowTrap>()) t.Fire();
    }
}
}
