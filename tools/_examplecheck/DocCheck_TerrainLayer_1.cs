namespace DocCheck_TerrainLayer
{
using Elintria;

public class FindLayer : ElintriaScript
{
    public static int IndexOf(TerrainData d, string name)
    {
        TerrainLayer[] layers = d.terrainLayers;
        for (int i = 0; i < layers.Length; i++) if (layers[i].name == name) return i;
        return -1;
    }
}
}
