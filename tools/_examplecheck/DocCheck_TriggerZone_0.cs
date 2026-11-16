namespace DocCheck_TriggerZone
{
using Elintria;
using Elintria.Standard;

public class PoisonCloud : ElintriaScript
{
    public override void Start()
    {
        TriggerZone z = GetComponent<TriggerZone>();
        z.size = new Vector3(6f, 3f, 6f);
        z.requiredTag = "Player";
        z.stayMessage = "ApplyDamage:4";
        z.stayInterval = 0.5f;
    }
}
}
