namespace DocCheck_VehicleController
{
using Elintria;
using Elintria.Standard;

public class BoostPad : ElintriaScript
{
    public float boost = 15f;

    public override void OnTriggerEnter(Collider other)
    {
        VehicleController car = other.gameObject.GetComponent<VehicleController>();
        if (car != null) car.velocity += car.Forward() * boost;
    }
}
}
