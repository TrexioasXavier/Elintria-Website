namespace DocCheck_WaterVolume
{
using Elintria;
using Elintria.Standard;

public class RisingFlood : ElintriaScript
{
    public float riseSpeed = 0.05f;

    public override void Update()
    {
        WaterVolume w = GetComponent<WaterVolume>();
        w.size = new Vector3(w.size.x, w.size.y + riseSpeed * Time.deltaTime, w.size.z);
    }
}
}
