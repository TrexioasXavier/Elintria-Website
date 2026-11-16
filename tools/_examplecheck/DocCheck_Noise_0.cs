namespace DocCheck_Noise
{
using Elintria;
using Elintria.Procedural;

public class NoiseTerrain : ElintriaScript
{
    public int seed = 3;

    public override void Start()
    {
        Terrain t = Terrain.activeTerrain;
        if (t == null) return;
        TerrainData d = t.terrainData;
        int n = d.heightmapResolution;
        Noise.Seed = seed;
        float[,] h = new float[n, n];
        for (int y = 0; y < n; y++)
            for (int x = 0; x < n; x++)
            {
                float hills = Noise.Fbm(x * 0.01f, y * 0.01f) * 0.5f + 0.5f;
                float ridges = Noise.Ridged(x * 0.005f, y * 0.005f);
                h[y, x] = hills * 0.4f + ridges * ridges * 0.4f;
            }
        d.SetHeights(0, 0, h);
    }
}
}
