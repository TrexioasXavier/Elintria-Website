namespace DocCheck_WaterVolume
{
using Elintria;
using Elintria.Standard;

public class Lava : ElintriaScript
{
    public override void Start() { GetComponent<WaterVolume>().damagePerSecond = 40f; }
}
}
