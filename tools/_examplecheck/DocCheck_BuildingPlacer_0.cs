namespace DocCheck_BuildingPlacer
{
using Elintria;
using Elintria.Standard;

public class CreativeMode : ElintriaScript
{
    public override void Start()
    {
        BuildingPlacer b = GetComponent<BuildingPlacer>();
        b.freeBuild = true;
        b.range = 20f;
    }
}
}
