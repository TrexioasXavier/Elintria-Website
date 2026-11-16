namespace DocCheck_Spline
{
using Elintria;

public class Tram : ElintriaScript
{
    public GameObject track;
    public float speed = 6f;
    float d;

    public override void Update()
    {
        Spline s = Spline.Get(track);
        if (s == null) return;
        d = (d + speed * Time.deltaTime) % s.length;
        transform.position = s.EvaluateAtDistance(d);
        transform.rotation = Quaternion.LookRotation(s.TangentAtDistance(d));
    }
}
}
