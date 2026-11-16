namespace DocCheck_FindObjectsSortMode
{
using Elintria;

public class Checkpoint : ElintriaScript { }

public class AllCheckpoints : ElintriaScript
{
    public override void Start()
    {
        Checkpoint[] cps = FindObjectsByType<Checkpoint>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        Debug.Log(cps.Length + " checkpoints, active or not");
    }
}
}
