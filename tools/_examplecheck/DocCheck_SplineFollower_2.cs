namespace DocCheck_SplineFollower
{
using Elintria;
using Elintria.Standard;

public class PathGuard : ElintriaScript
{
    public bool alerted;

    public override void Start()
    {
        SplineFollower f = GetComponent<SplineFollower>();
        f.mode = 1;   // ping-pong along the wall
        f.offset = new Vector3(0f, 0.05f, 0f);
    }

    public override void Update()
    {
        GetComponent<SplineFollower>().speed = alerted ? 4f : 1.5f;
    }
}
}
