namespace DocCheck_LightType
{
using Elintria;

public class LightMode : ElintriaScript
{
    public override void Update()
    {
        Light l = GetComponent<Light>();
        if (Input.GetKeyDown(KeyCode.Alpha1)) l.type = LightType.Point;
        if (Input.GetKeyDown(KeyCode.Alpha2)) l.type = LightType.Spot;
        if (Input.GetKeyDown(KeyCode.Alpha3)) l.type = LightType.Directional;
    }
}
}
