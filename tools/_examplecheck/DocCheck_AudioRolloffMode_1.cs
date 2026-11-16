namespace DocCheck_AudioRolloffMode
{
using Elintria;

public class Gunfire : ElintriaScript
{
    public override void Start()
    {
        AudioSource s = gameObject.audioSource;
        s.spatialBlend = 1f;
        s.rolloffMode = AudioRolloffMode.Logarithmic;
        s.minDistance = 5f;
    }

    public void Shoot() { gameObject.audioSource.PlayOneShot("Audio/shot.wav"); }
}
}
