namespace DocCheck_ParticleBlend
{
using Elintria;

public class MagicGlow : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.blendMode = ParticleBlend.Additive;
        p.startColor = new Color(0.4f, 0.6f, 1f, 1f);
        p.texture = "glow";
    }
}
}
