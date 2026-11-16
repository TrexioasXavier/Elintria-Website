namespace DocCheck_BasicEnemy
{
using Elintria;
using Elintria.Standard;

public class NightBuff : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        BasicEnemy e = GetComponent<BasicEnemy>();
        e.runSpeed = d != null && !d.isDay ? 6f : 4.5f;
    }
}
}
