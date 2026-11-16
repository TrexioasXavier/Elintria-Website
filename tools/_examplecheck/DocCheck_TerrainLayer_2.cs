namespace DocCheck_TerrainLayer
{
using Elintria;

public class PaintPath : ElintriaScript
{
    public string layerName = "Dirt";

    public override void Update()
    {
        TerrainData d = Terrain.activeTerrain.terrainData;
        int layer = FindLayer.IndexOf(d, layerName);
        if (layer < 0) return;
        Vector3 local = transform.position - Terrain.activeTerrain.GetPosition();
        int x = (int)(local.x / d.size.x * d.alphamapWidth), z = (int)(local.z / d.size.z * d.alphamapHeight);
        if (x < 0 || z < 0 || x >= d.alphamapWidth || z >= d.alphamapHeight) return;
        float[,,] w = new float[1, 1, d.alphamapLayers];
        w[0, 0, layer] = 1f;
        d.SetAlphamaps(x, z, w);
    }
}
}
