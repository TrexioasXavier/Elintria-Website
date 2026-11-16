namespace DocCheck_VehicleEntry
{
using Elintria;
using Elintria.Standard;

public class DoorSeat : ElintriaScript
{
    public GameObject car;

    public override void Start()
    {
        VehicleEntry e = gameObject.AddComponent<VehicleEntry>();
        e.vehicle = car;
        e.prompt = "Get in";
    }
}
}
