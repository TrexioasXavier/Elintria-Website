namespace DocCheck_TerrainCollider
{
using Elintria;

public class TerrainSize : ElintriaScript
{
    public override void Start()
    {
        TerrainCollider tc = GetComponent<TerrainCollider>();
        if (tc != null) Debug.Log("Terrain is " + tc.terrainData.size);
    }
}
}
