namespace DocCheck_BuildingPlacer
{
using Elintria;
using Elintria.Standard;

public class BuildCount : ElintriaScript
{
    public override void Update()
    {
        BuildingPlacer b = GetComponent<BuildingPlacer>();
        if (b.building) ScreenGUI.Text(20f, 180f, b.placed.Count + " pieces built", null, Color.white);
    }
}
}
