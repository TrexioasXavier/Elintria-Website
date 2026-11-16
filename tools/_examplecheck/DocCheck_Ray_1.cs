namespace DocCheck_Ray
{
using Elintria;

public class TeleportAim : ElintriaScript
{
    public float maxDistance = 12f;

    public override void Update()
    {
        Ray aim = new Ray(transform.position + Vector3.up, transform.forward);
        Vector3 spot = Physics.Raycast(aim, out RaycastHit hit, maxDistance) ? hit.point : aim.GetPoint(maxDistance);
        Debug.DrawLine(aim.origin, spot, Color.magenta);
        if (Input.GetKeyDown(KeyCode.T)) transform.position = spot;
    }
}
}
