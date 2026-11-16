namespace DocCheck_SwingingBlade
{
using Elintria;
using Elintria.Standard;

public class AlarmBlades : ElintriaScript
{
    public override void Update() { GetComponent<SwingingBlade>().period = StdVars.Is("alarm", "1") ? 1.2f : 2.4f; }
}
}
