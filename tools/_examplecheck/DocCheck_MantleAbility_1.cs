namespace DocCheck_MantleAbility
{
using Elintria;
using Elintria.Standard;

public class TallClimber : ElintriaScript
{
    public override void Start()
    {
        MantleAbility m = GetComponent<MantleAbility>();
        m.maxHeight = 2.8f;
        m.duration = 0.35f;
    }
}
}
