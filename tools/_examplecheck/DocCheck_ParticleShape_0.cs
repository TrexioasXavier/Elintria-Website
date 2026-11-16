namespace DocCheck_ParticleShape
{
using Elintria;

public class ExplosionShape : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.shape = ParticleShape.Sphere;
        p.shapeRadius = 0.5f;
        p.Emit(80);
    }
}
}
