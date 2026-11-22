namespace DocCheck_BallisticFlight
{
using Elintria;
using Elintria.Standard;

public class StopFlightOnKey : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && BallisticFlight.IsFlying(gameObject))
            BallisticFlight.Cancel(gameObject);       // the controller's own gravity takes over
    }
}
}
