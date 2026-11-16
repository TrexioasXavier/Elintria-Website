namespace DocCheck_Audio
{
using Elintria;

public class BoomSound : ElintriaScript
{
    public void Explode()
    {
        Audio.PlayClipAtPoint("Audio/explosion.wav", transform.position, 1f);
        Destroy(gameObject);
    }
}
}
