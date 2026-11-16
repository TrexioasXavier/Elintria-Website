namespace DocCheck_ParticleEmitter
{
using Elintria;

public class ImpactBurst : ElintriaScript
{
    public GameObject effect;   // has a ParticleEmitter, not looping

    public override void OnCollisionEnter(Collision collision)
    {
        ParticleEmitter p = effect.GetComponent<ParticleEmitter>();
        effect.transform.position = collision.GetContact(0).point;
        p.startColor = Color.yellow;
        p.Emit(Mathf.Clamp((int)collision.relativeVelocity.magnitude * 4, 5, 60));
    }
}
}
