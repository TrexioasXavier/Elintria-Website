namespace DocCheck_Water
{
using Elintria;

public class Swimmer : ElintriaScript
{
    public bool swimming;

    public override void Update()
    {
        float depth = Water.Depth(transform.position + Vector3.up * 1.2f);   // chest height
        swimming = depth > 0f;
        if (Water.IsUnderwater(transform.position + Vector3.up * 1.7f))       // head
            RenderEnvironment.SetFog(0.1f, 0.3f, 0.4f, 0f, 15f);
        else
            RenderEnvironment.SetFog(0f, 0f, 0f, 1f, 0f);                     // fog off
    }
}
}
