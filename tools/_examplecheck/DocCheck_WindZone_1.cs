namespace DocCheck_WindZone
{
using Elintria;
using Elintria.Standard;

public class TurningWind : ElintriaScript
{
    public float degreesPerSecond = 10f;

    public override void Update()
    {
        WindZone w = GetComponent<WindZone>();
        w.direction = Quaternion.Euler(0f, degreesPerSecond * Time.deltaTime, 0f) * w.direction;
    }
}
}
