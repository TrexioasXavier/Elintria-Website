namespace DocCheck_FindObjectsInactive
{
using Elintria;

public class Turret2 : ElintriaScript { }

public class ActiveTurrets : ElintriaScript
{
    public override void Start()
    {
        Debug.Log(FindObjectsByType<Turret2>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length + " turrets on");
    }
}
}
