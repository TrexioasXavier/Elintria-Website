namespace DocCheck_WaterVolume
{
using Elintria;
using Elintria.Standard;

public class WetCheck : ElintriaScript
{
    public override void Update()
    {
        float y; WaterVolume v;
        if (WaterVolume.At(transform.position, out y, out v)) Debug.Log("In water, surface at " + y);
    }
}
}
