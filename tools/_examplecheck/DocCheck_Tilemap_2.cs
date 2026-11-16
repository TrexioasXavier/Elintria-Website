namespace DocCheck_Tilemap
{
using Elintria;

public class TileGrounded : ElintriaScript
{
    public GameObject level;

    public bool Grounded()
    {
        Tilemap map = Tilemap.Get(level);
        return map != null && map.HasTile(transform.position + Vector3.down * 0.55f);
    }
}
}
