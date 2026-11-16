namespace DocCheck_ParticleRenderMode
{
using Elintria;

public class SparkStreaks : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.renderMode = ParticleRenderMode.Stretched;
        p.texture = "spark";
        p.blendMode = ParticleBlend.Additive;
    }
}
}
