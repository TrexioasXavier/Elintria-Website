namespace DocCheck_ParticleRenderMode
{
using Elintria;

public class Tufts : ElintriaScript
{
    public override void Start() { GetComponent<ParticleEmitter>().renderMode = ParticleRenderMode.Vertical; }
}
}
