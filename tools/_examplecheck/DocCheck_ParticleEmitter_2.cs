namespace DocCheck_ParticleEmitter
{
using Elintria;

public class Campfire : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter fire = GetComponent<ParticleEmitter>();
        fire.shape = ParticleShape.Cone;
        fire.shapeRadius = 0.3f;
        fire.texture = "flare";
        fire.blendMode = ParticleBlend.Additive;
        fire.startColor = new Color(1f, 0.5f, 0.1f, 1f);
        fire.gravityModifier = -0.2f;              // rises
        fire.Set("startLifetime", "0.6,1.2");      // a random range, by key
        fire.Simulate(2f);                          // start already burning
        fire.Play();
        Debug.Log("Particles now: " + fire.particleCount);
    }
}
}
