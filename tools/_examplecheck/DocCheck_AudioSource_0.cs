namespace DocCheck_AudioSource
{
using System.Collections;
using Elintria;

public class Music : ElintriaScript
{
    public string track = "Audio/theme.ogg";
    AudioSource src;

    public override void Start()
    {
        src = gameObject.audioSource;
        src.clip = track;
        src.loop = true;
        src.spatialBlend = 0f;   // 2D: same volume everywhere
        src.volume = 0f;
        src.Play();
        StartCoroutine(Fade(0.6f, 3f));
    }

    public IEnumerator Fade(float to, float seconds)
    {
        float from = src.volume;
        for (float t = 0f; t < 1f; t += Time.deltaTime / seconds) { src.volume = Mathf.Lerp(from, to, t); yield return null; }
        src.volume = to;
    }
}
}
