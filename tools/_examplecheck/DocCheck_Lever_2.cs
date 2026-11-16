namespace DocCheck_Lever
{
using Elintria;
using Elintria.Standard;

public class BridgeLever : ElintriaScript
{
    public GameObject bridge;

    public override void Start()
    {
        Lever l = GetComponent<Lever>();
        l.oneShot = true;
        l.throwAngle = 60f;
        l.onOnTargets.Add(bridge);
        l.onOnMessage = "Activate";   // a MovingPlatform waiting for Activate
    }
}
}
