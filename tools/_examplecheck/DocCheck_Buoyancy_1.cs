namespace DocCheck_Buoyancy
{
using Elintria;
using Elintria.Standard;

public class BuoySetup : ElintriaScript
{
    public override void Start()
    {
        Buoyancy b = GetComponent<Buoyancy>();
        b.draft = 0.4f;   // sits 0.4 m deep
        b.tilt = 0.8f;    // follows the waves' slope
    }
}
}
