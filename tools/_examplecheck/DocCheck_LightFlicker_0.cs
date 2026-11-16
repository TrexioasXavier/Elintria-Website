namespace DocCheck_LightFlicker
{
using Elintria;
using Elintria.Standard;

public class Alarm : ElintriaScript
{
    public LightFlicker[] lights;

    public void RaiseAlarm()
    {
        foreach (LightFlicker f in lights) { f.mode = 1; f.rate = 2f; f.minimum = 0f; }
    }
}
}
