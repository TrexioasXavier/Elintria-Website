namespace DocCheck_TriggerZone
{
using Elintria;
using Elintria.Standard;

public class Checkpoint2 : ElintriaScript
{
    public static Vector3 lastCheckpoint;

    public override void Start()
    {
        TriggerZone z = GetComponent<TriggerZone>();
        z.requiredTag = "Player";
        z.once = true;
        z.onEnterTargets.Add(gameObject);
        z.onEnterMessage = "Reached";
    }

    public void Reached(GameObject player)
    {
        lastCheckpoint = transform.position;
        player.GetComponent<Interactor>()?.Say("Checkpoint reached");
    }
}
}
