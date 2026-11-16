namespace DocCheck_TerrainLayer
{
using Elintria;

public class LayerList : ElintriaScript
{
    public override void Start()
    {
        foreach (TerrainLayer l in Terrain.activeTerrain.terrainData.terrainLayers)
            Debug.Log(l.name + " tiles every " + l.tileSize);
    }
}
}
