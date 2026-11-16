namespace DocCheck_VehicleController
{
using Elintria;
using Elintria.Standard;

public class CarSetup : ElintriaScript
{
    public int setup;   // 0 grippy, 1 drifty, 2 off-road

    public override void Start()
    {
        VehicleController c = GetComponent<VehicleController>();
        if (setup == 0) { c.grip = 10f; c.driftGrip = 4f; c.maxSpeed = 32f; }
        else if (setup == 1) { c.grip = 5f; c.driftGrip = 0.8f; c.maxSteerAngle = 38f; }
        else { c.rideHeight = 0.8f; c.suspensionTravel = 0.5f; c.maxStepHeight = 0.6f; c.maxSpeed = 22f; }
    }
}
}
