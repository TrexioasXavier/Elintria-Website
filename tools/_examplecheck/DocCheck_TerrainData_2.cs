namespace DocCheck_TerrainData
{
using Elintria;

public class GroundType : ElintriaScript
{
    public string Underfoot()
    {
        Terrain t = Terrain.activeTerrain;
        TerrainData d = t.terrainData;
        Vector3 local = transform.position - t.GetPosition();
        int ax = Mathf.Clamp((int)(local.x / d.size.x * d.alphamapWidth), 0, d.alphamapWidth - 1);
        int az = Mathf.Clamp((int)(local.z / d.size.z * d.alphamapHeight), 0, d.alphamapHeight - 1);
        float[,,] w = d.GetAlphamaps(ax, az, 1, 1);
        int best = 0;
        for (int l = 1; l < d.alphamapLayers; l++) if (w[0, 0, l] > w[0, 0, best]) best = l;
        return d.terrainLayers[best].name;
    }
}
}
