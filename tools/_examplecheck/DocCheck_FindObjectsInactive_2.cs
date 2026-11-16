namespace DocCheck_FindObjectsInactive
{
using Elintria;

public class Boss : ElintriaScript { }

public class AnyBoss : ElintriaScript
{
    public override void Start()
    {
        Boss b = FindAnyObjectByType<Boss>(FindObjectsInactive.Include);
        if (b != null && !b.gameObject.activeSelf) Debug.Log("The boss is waiting, inactive, in the scene");
    }
}
}
