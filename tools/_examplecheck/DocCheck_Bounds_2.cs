namespace DocCheck_Bounds
{
using Elintria;

public class ProximityAlarm : ElintriaScript
{
    public GameObject vault;   // has a collider
    public float alarmDistance = 3f;

    public override void Update()
    {
        Collider c = vault != null ? vault.GetComponent<Collider>() : null;
        if (c == null) return;
        Bounds b = c.bounds;
        if (b.SqrDistance(transform.position) < alarmDistance * alarmDistance)
            Debug.DrawLine(transform.position, b.ClosestPoint(transform.position), Color.red);
    }
}
}
