namespace DocCheck_TerrainData
{
using Elintria;

public class Crater : ElintriaScript
{
    public float radiusSamples = 6f;
    public float depth = 0.01f;

    public void Dig(Vector3 world)
    {
        Terrain t = Terrain.activeTerrain;
        TerrainData d = t.terrainData;
        Vector3 local = world - t.GetPosition();
        int cx = (int)(local.x / d.size.x * (d.heightmapResolution - 1));
        int cz = (int)(local.z / d.size.z * (d.heightmapResolution - 1));
        int r = (int)radiusSamples, x0 = Mathf.Max(0, cx - r), z0 = Mathf.Max(0, cz - r);
        int w = Mathf.Min(d.heightmapResolution - x0, r * 2), h = Mathf.Min(d.heightmapResolution - z0, r * 2);
        float[,] hs = d.GetHeights(x0, z0, w, h);
        for (int z = 0; z < h; z++)
            for (int x = 0; x < w; x++)
            {
                float dist = new Vector2(x0 + x - cx, z0 + z - cz).magnitude / radiusSamples;
                if (dist < 1f) hs[z, x] -= depth * (1f - dist * dist);
            }
        d.SetHeights(x0, z0, hs);
    }
}
}
