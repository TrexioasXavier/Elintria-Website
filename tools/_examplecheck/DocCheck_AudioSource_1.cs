namespace DocCheck_AudioSource
{
using Elintria;

public class Footsteps : ElintriaScript
{
    public float stepInterval = 0.45f;
    float next;
    AudioSource src;

    public override void Start()
    {
        src = gameObject.audioSource;
        src.spatialBlend = 1f;
        src.minDistance = 1f;
        src.maxDistance = 20f;
    }

    public override void Update()
    {
        bool moving = Mathf.Abs(Input.GetAxisVertical()) + Mathf.Abs(Input.GetAxisHorizontal()) > 0.1f;
        if (!moving || Time.time < next) return;
        next = Time.time + stepInterval;
        src.pitch = Random.Range(0.9f, 1.1f);
        src.PlayOneShot("Audio/step_" + Random.Range(1, 5) + ".wav", 0.8f);
    }
}
}
