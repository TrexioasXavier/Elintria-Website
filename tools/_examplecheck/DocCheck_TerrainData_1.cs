namespace DocCheck_TerrainData
{
using Elintria;

public class SlopeSpeed : ElintriaScript
{
    public float SpeedFactor()
    {
        Terrain t = Terrain.activeTerrain;
        if (t == null) return 1f;
        TerrainData d = t.terrainData;
        Vector3 local = transform.position - t.GetPosition();
        float steep = d.GetSteepness(local.x / d.size.x, local.z / d.size.z);
        return Mathf.Lerp(1f, 0.4f, Mathf.InverseLerp(15f, 45f, steep));
    }
}
}
