namespace DocCheck_SwingingBlade
{
using Elintria;
using Elintria.Standard;

public class BladeSwitch : ElintriaScript
{
    public void Stop() { GetComponent<SwingingBlade>().enabled = false; }
}
}
