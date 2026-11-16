namespace DocCheck_AudioSource
{
using Elintria;

public class EngineHum : ElintriaScript
{
    AudioSource src;
    Rigidbody rb;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        src = AudioSource.Add(gameObject);
        src.clip = "Audio/engine_loop.wav";
        src.loop = true;
        src.spatialBlend = 1f;
        src.rolloffMode = AudioRolloffMode.Linear;
        src.maxDistance = 60f;
        src.Play();
    }

    public override void Update()
    {
        float speed = rb != null ? rb.velocity.magnitude : 0f;
        src.pitch = Mathf.Lerp(0.8f, 2f, speed / 30f);
        src.volume = Mathf.Lerp(0.4f, 1f, speed / 30f);
    }
}
}
