namespace DocCheck_ParticleBlend
{
using Elintria;

public class FireflyBlend : ElintriaScript
{
    public bool night;

    public override void Update()
    {
        GetComponent<ParticleEmitter>().blendMode = night ? ParticleBlend.Additive : ParticleBlend.AlphaBlended;
    }
}
}
