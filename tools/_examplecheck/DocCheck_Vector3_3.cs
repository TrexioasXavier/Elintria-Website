namespace DocCheck_Vector3
{
using Elintria;

public class OrbitRig : ElintriaScript
{
    public GameObject target;
    public float distance = 8f;
    public float height = 3f;

    Vector3 vel;

    public override void LateUpdate()
    {
        if (target == null) return;
        Vector3 fwd = target.transform.forward;
        Vector3 side = Vector3.Cross(Vector3.up, fwd).normalized;   // the target's right
        Vector3 wanted = target.transform.position - fwd * distance + Vector3.up * height + side * 1.5f;
        transform.position = Vector3.SmoothDamp(transform.position, wanted, ref vel, 0.3f);
    }
}
}
