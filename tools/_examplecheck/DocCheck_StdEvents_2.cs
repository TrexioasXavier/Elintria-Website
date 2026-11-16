namespace DocCheck_StdEvents
{
using Elintria;
using Elintria.Standard;

public class BossMusic : ElintriaScript
{
    public GameObject boss;
    public AudioSource music;

    public override void Start()
    {
        StdEvents.Damaged += (h, amount, attacker) =>
        {
            if (h.gameObject == boss && music != null && !music.isPlaying) music.Play();
        };
    }
}
}
