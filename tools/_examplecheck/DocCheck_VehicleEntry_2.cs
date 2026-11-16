namespace DocCheck_VehicleEntry
{
using Elintria;
using Elintria.Standard;

public class EngineStart : ElintriaScript
{
    bool wasDriving;

    public override void Update()
    {
        bool d = GetComponent<VehicleController>().driving;
        if (d && !wasDriving) gameObject.audioSource.PlayOneShot("Audio/engine_start.wav");
        wasDriving = d;
    }
}
}
