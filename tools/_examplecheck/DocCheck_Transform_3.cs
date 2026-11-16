namespace DocCheck_Transform
{
using Elintria;

public class MuzzleFlash : ElintriaScript
{
    public Vector3 muzzleLocal = new Vector3(0f, 0.1f, 0.6f);   // the barrel tip, in the gun's space

    public Vector3 MuzzleWorld()
    {
        return transform.TransformPoint(muzzleLocal);
    }

    public Vector3 ShotDirection()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    public bool IsInFront(Vector3 worldPoint)
    {
        // In local space, "in front" simply means z > 0.
        return transform.InverseTransformPoint(worldPoint).z > 0f;
    }

    public override void Update()
    {
        Debug.DrawRay(MuzzleWorld(), ShotDirection() * 5f, Color.yellow);
    }
}
}
