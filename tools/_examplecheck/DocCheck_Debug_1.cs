namespace DocCheck_Debug
{
using Elintria;

public class LaserSight : ElintriaScript
{
    public float range = 30f;

    public override void Update()
    {
        Vector3 from = transform.position;
        Vector3 dir = transform.forward;
        if (Physics.Raycast(from, dir, out RaycastHit hit, range))
            Debug.DrawLine(from, hit.point, Color.green);
        else
            Debug.DrawRay(from, dir * range, Color.red);
    }
}
}
