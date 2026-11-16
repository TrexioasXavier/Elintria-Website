namespace DocCheck_TriggerZone
{
using Elintria;
using Elintria.Standard;

public class MusicZone : ElintriaScript
{
    public string track = "Audio/cave_theme.ogg";

    public override void Start()
    {
        TriggerZone z = GetComponent<TriggerZone>();
        z.requiredTag = "Player";
        z.onEnterTargets.Add(gameObject); z.onEnterMessage = "Enter";
        z.onExitTargets.Add(gameObject);  z.onExitMessage = "Leave";
    }

    public void Enter() { AudioSource s = gameObject.audioSource; s.clip = track; s.loop = true; s.Play(); }
    public void Leave() { gameObject.audioSource.Stop(); }
}
}
