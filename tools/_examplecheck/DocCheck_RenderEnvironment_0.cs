namespace DocCheck_RenderEnvironment
{
using Elintria;

public class Swamp : ElintriaScript
{
    public override void Start()
    {
        RenderEnvironment.SetSky(0.45f, 0.5f, 0.45f);
        RenderEnvironment.SetFog(0.4f, 0.45f, 0.4f, 5f, 60f);
        RenderEnvironment.SetHeightFog(0.35f, 0.4f, 0.35f, 1.5f, 3f);
    }
}
}
