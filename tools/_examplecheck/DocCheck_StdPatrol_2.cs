namespace DocCheck_StdPatrol
{
using Elintria;
using Elintria.Standard;

public class PatrolDebug : ElintriaScript
{
    public override void Update()
    {
        StdPatrol p = GetComponent<StdPatrol>();
        if (p.waypoints.Count > 0 && Input.GetKeyDown(KeyCode.P))
            Debug.Log(gameObject.name + " heads for " + p.waypoints[p.current].name);
    }
}
}
