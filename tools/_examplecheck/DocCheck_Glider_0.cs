namespace DocCheck_Glider
{
using Elintria;
using Elintria.Standard;

public class HangGlider : ElintriaScript
{
    public override void Start()
    {
        Glider g = GetComponent<Glider>();
        g.glideSpeed = 7f;
        g.sinkSpeed = 1.2f;
        g.minHeight = 4f;
    }
}
}
