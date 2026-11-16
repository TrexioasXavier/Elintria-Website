namespace DocCheck_RollingBoulder
{
using Elintria;
using Elintria.Standard;

public class ManualBoulder : ElintriaScript
{
    public override void Start() { GetComponent<RollingBoulder>().triggerDistance = 0f; }
}
}
