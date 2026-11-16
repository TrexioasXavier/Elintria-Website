namespace DocCheck_Matrix4x4
{
using Elintria;

public class LocalHit : ElintriaScript
{
    public override void OnCollisionEnter(Collision collision)
    {
        Vector3 world = collision.GetContact(0).point;
        Vector3 local = transform.worldToLocalMatrix.MultiplyPoint3x4(world);
        Vector3 back = transform.localToWorldMatrix.MultiplyPoint3x4(local);
        Debug.Log("Hit at local " + local + " (world " + back + ")");
    }
}
}
