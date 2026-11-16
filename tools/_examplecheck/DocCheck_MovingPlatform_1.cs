namespace DocCheck_MovingPlatform
{
using Elintria;
using Elintria.Standard;

public class BobPlatform : ElintriaScript
{
    public override void Start()
    {
        MovingPlatform m = GetComponent<MovingPlatform>();
        m.endOffset = new Vector3(0f, 4f, 0f);
        m.speed = 2f;
        m.waitTime = 1f;
        m.pingPong = true;
    }
}
}
