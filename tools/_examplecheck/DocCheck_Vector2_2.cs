namespace DocCheck_Vector2
{
using Elintria;

public class Asteroids2D : ElintriaScript
{
    public GameObject target;
    public Vector2 heading = Vector2.up;
    public float turnRate = 180f;

    public override void Update()
    {
        if (target == null) return;
        Vector2 to = (Vector2)(target.transform.position - transform.position);
        float turn = Mathf.Clamp(Vector2.SignedAngle(heading, to), -turnRate * Time.deltaTime, turnRate * Time.deltaTime);
        float r = turn * Mathf.Deg2Rad;
        heading = new Vector2(heading.x * Mathf.Cos(r) - heading.y * Mathf.Sin(r), heading.x * Mathf.Sin(r) + heading.y * Mathf.Cos(r));
        Vector2 strafe = Vector2.Perpendicular(heading) * Mathf.Sin(Time.time) * 0.5f;
        Vector3 step = (heading * 3f + strafe) * Time.deltaTime;
        transform.position += step;
    }
}
}
