namespace DocCheck_ParticleBlend
{
using Elintria;

public class Soot : ElintriaScript
{
    public override void Start() { GetComponent<ParticleEmitter>().blendMode = ParticleBlend.Multiply; }
}
}
