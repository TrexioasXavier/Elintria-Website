namespace DocCheck_Terrain
{
using Elintria;

public class RockScatter : ElintriaScript
{
    public GameObject rockTemplate;
    public int perTerrain = 40;

    public override void Start()
    {
        foreach (Terrain t in Terrain.activeTerrains)
        {
            Vector3 corner = t.GetPosition(), size = t.terrainData.size;
            for (int i = 0; i < perTerrain; i++)
            {
                Vector3 p = corner + new Vector3(Random.value * size.x, 0f, Random.value * size.z);
                p.y = corner.y + t.SampleHeight(p);
                Instantiate(rockTemplate, p, Random.rotation).SetActive(true);
            }
        }
    }
}
}
