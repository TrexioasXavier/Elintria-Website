namespace DocCheck_Terrain
{
using Elintria;

public class XRayView : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && Terrain.activeTerrain != null)
            Terrain.activeTerrain.drawHeightmap = !Terrain.activeTerrain.drawHeightmap;   // its collider still works
    }
}
}
