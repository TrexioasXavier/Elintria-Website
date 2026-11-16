namespace DocCheck_ParticleRenderMode
{
using Elintria;

public class Ripples : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.renderMode = ParticleRenderMode.Horizontal;
        p.texture = "ring";
        p.startSpeed = 0f;
    }
}
}
