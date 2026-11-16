namespace DocCheck_MantleAbility
{
using Elintria;
using Elintria.Standard;

public class HeavyLoad : ElintriaScript
{
    public override void Update()
    {
        GetComponent<MantleAbility>().enabled = GetComponent<PhysicsGrabber>().held == null;
    }
}
}
