namespace DocCheck_WindZone
{
using Elintria;
using Elintria.Standard;

public class Fan : ElintriaScript
{
    public void On() { GetComponent<WindZone>().enabled = true; }
    public void Off() { GetComponent<WindZone>().enabled = false; }
}
}
