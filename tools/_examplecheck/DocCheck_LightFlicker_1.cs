namespace DocCheck_LightFlicker
{
using Elintria;
using Elintria.Standard;

public class PowerFailing : ElintriaScript
{
    public override void Update()
    {
        LightFlicker f = GetComponent<LightFlicker>();
        f.rate = Mathf.Min(6f, 0.5f + Time.time / 60f);   // more often each minute
    }
}
}
