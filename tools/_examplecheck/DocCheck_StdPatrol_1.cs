namespace DocCheck_StdPatrol
{
using Elintria;
using Elintria.Standard;

public class RouteBuilder : ElintriaScript
{
    public GameObject a, b, c;

    public override void Start()
    {
        StdPatrol p = GetComponent<StdPatrol>();
        p.waypoints.Clear();
        p.waypoints.Add(a); p.waypoints.Add(b); p.waypoints.Add(c);
        p.loop = false;          // a, b, c, b, a ...
        p.waitTime = 3f;
    }
}
}
