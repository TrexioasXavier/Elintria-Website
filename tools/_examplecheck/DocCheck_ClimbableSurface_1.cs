namespace DocCheck_ClimbableSurface
{
using Elintria;
using Elintria.Standard;

public class WitheringIvy : ElintriaScript
{
    public float lasts = 60f;

    public override void Update()
    {
        if (Time.time > lasts) GetComponent<ClimbableSurface>().enabled = false;
    }
}
}
