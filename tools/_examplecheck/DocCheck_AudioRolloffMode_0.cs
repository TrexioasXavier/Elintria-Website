namespace DocCheck_AudioRolloffMode
{
using Elintria;

public class Radio : ElintriaScript
{
    public override void Start()
    {
        AudioSource s = gameObject.audioSource;
        s.clip = "Audio/radio.ogg";
        s.loop = true;
        s.spatialBlend = 1f;
        s.rolloffMode = AudioRolloffMode.Linear;
        s.minDistance = 2f;
        s.maxDistance = 15f;   // silent beyond 15 m
        s.Play();
    }
}
}
