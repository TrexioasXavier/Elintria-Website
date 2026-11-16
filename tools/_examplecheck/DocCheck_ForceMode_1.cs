namespace DocCheck_ForceMode
{
using Elintria;

public class WindZone2 : ElintriaScript
{
    public Vector3 wind = new Vector3(3f, 0f, 0f);

    public override void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null) rb.AddForce(wind, ForceMode.Acceleration);
    }
}
}
