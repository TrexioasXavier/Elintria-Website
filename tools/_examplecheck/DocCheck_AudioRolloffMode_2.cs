namespace DocCheck_AudioRolloffMode
{
using Elintria;

public class RolloffToggle : ElintriaScript
{
    public bool arcadeAudio = true;

    public override void Start()
    {
        foreach (AudioSource s in FindObjectsOfType<AudioSource>())
            s.rolloffMode = arcadeAudio ? AudioRolloffMode.Linear : AudioRolloffMode.Logarithmic;
    }
}
}
