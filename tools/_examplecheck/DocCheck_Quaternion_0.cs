namespace DocCheck_Quaternion
{
using Elintria;

public class SmoothTurret : ElintriaScript
{
    public GameObject target;
    public float degreesPerSecond = 90f;

    public override void Update()
    {
        if (target == null) return;
        Vector3 to = target.transform.position - transform.position;
        to.y = 0f;                                   // turn only left and right
        if (to.sqrMagnitude < 0.0001f) return;
        Quaternion wanted = Quaternion.LookRotation(to);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted, degreesPerSecond * Time.deltaTime);
    }
}
}
