namespace DocCheck_Crosshair
{
using Elintria;
using Elintria.Standard;

public class DotSight : ElintriaScript
{
    public override void Start()
    {
        Crosshair c = GetComponent<Crosshair>();
        c.color = new Color(0.3f, 1f, 0.4f, 1f);
        c.length = 0f;
        c.dot = true;
    }
}
}
