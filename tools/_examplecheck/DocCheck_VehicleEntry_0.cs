namespace DocCheck_VehicleEntry
{
using Elintria;
using Elintria.Standard;

public class CarKey : ElintriaScript
{
    public string keyItem = "car_key";

    public override void Update()
    {
        VehicleEntry seat = GetComponent<VehicleEntry>();
        GameObject player = GameObject.FindWithTag("Player");
        Inventory inv = player != null ? player.GetComponent<Inventory>() : null;
        seat.interactable = inv != null && inv.Has(keyItem);
        seat.prompt = seat.interactable ? "Drive" : "Locked";
    }
}
}
