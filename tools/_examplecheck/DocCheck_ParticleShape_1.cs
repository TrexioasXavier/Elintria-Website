namespace DocCheck_ParticleShape
{
using Elintria;

public class RainBox : ElintriaScript
{
    public GameObject player;

    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.shape = ParticleShape.Box;
        p.texture = "drop";
        p.rateOverTime = 400f;
        p.gravityModifier = 2f;
    }

    public override void Update()
    {
        if (player != null) transform.position = player.transform.position + Vector3.up * 12f;
    }
}
}
