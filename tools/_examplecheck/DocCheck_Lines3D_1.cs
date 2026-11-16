namespace DocCheck_Lines3D
{
using Elintria;

public class LaserBeam : ElintriaScript
{
    public override void Update()
    {
        Vector3 a = transform.position;
        Vector3 b = Physics.Raycast(a, transform.forward, out RaycastHit hit, 50f) ? hit.point : a + transform.forward * 50f;
        Lines3D.Submit(new[] { a.x, a.y, a.z, b.x, b.y, b.z, 1f, 0.1f, 0.1f, 1f }, 1, 3f);
    }
}
}
