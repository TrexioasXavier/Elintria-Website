namespace DocCheck_Terrain
{
using Elintria;

public class SnapToTerrain : ElintriaScript
{
    public float offset = 0f;

    public override void LateUpdate()
    {
        Terrain t = Terrain.activeTerrain;
        if (t == null) return;
        Vector3 p = transform.position;
        p.y = t.SampleHeight(p) + t.GetPosition().y + offset;
        transform.position = p;
    }
}
}
