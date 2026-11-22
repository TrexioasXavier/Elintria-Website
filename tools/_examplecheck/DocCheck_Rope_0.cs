namespace DocCheck_Rope
{
using Elintria;

public class WreckingBall : ElintriaScript
{
    public GameObject ball;

    public override void Start()
    {
        if (ball == null) return;
        Rope r = gameObject.AddComponent<Rope>();
        r.endObject = ball;
        r.segments = 20;
        r.thickness = 0.08f;
    }
}
}
