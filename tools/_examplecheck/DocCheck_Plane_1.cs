namespace DocCheck_Plane
{
using Elintria;

public class WallMarker : ElintriaScript
{
    public GameObject wall;
    public GameObject marker;

    public override void Update()
    {
        Plane p = new Plane(wall.transform.forward, wall.transform.position);
        marker.transform.position = p.ClosestPointOnPlane(transform.position);
        float d = p.GetDistanceToPoint(transform.position);
        ScreenGUI.Text(20, 40, "Distance to wall: " + d.ToString("0.0"));
    }
}
}
