namespace DocCheck_ParticleEmitter
{
using Elintria;

public class ExhaustSmoke : ElintriaScript
{
    ParticleEmitter smoke;

    public override void Start()
    {
        smoke = GetComponent<ParticleEmitter>();
        smoke.texture = "smoke";
        smoke.blendMode = ParticleBlend.AlphaBlended;
        smoke.looping = true;
        smoke.Play();
    }

    public override void Update()
    {
        float throttle = Mathf.Abs(Input.GetAxisVertical());
        smoke.rateOverTime = Mathf.Lerp(4f, 40f, throttle);
        smoke.startSpeed = Mathf.Lerp(0.5f, 3f, throttle);
    }
}
}
