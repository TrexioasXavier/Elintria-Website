namespace DocCheck_Transform
{
using Elintria;

public class FollowCamera : ElintriaScript
{
    public GameObject target;
    public Vector3 offset = new Vector3(0f, 3f, -6f);   // behind and above, in the target's space
    public float smoothTime = 0.25f;

    Vector3 velocity;

    public override void LateUpdate()
    {
        if (target == null) return;
        Transform t = target.transform;
        Vector3 wanted = t.TransformPoint(offset);
        transform.position = Vector3.SmoothDamp(transform.position, wanted, ref velocity, smoothTime);

        // Cameras look down -Z: point +Z away from the target.
        Vector3 look = (t.position + Vector3.up) - transform.position;
        transform.rotation = Quaternion.LookRotation(-look);
    }
}
}
