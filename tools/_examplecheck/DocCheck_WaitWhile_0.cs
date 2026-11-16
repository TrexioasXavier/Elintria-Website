namespace DocCheck_WaitWhile
{
using System.Collections;
using Elintria;

public class Announcer : ElintriaScript
{
    public override void Start() { StartCoroutine(Say()); }

    IEnumerator Say()
    {
        AudioSource src = gameObject.audioSource;
        src.clip = "Audio/round_one.wav";
        src.Play();
        yield return new WaitWhile(() => src.isPlaying);
        Debug.Log("Fight!");
    }
}
}
