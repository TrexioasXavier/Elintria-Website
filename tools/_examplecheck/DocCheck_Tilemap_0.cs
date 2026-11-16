namespace DocCheck_Tilemap
{
using Elintria;

public class BlockBreaker : ElintriaScript
{
    public GameObject level;

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        Tilemap map = Tilemap.Get(level);
        Vector3 below = transform.position + Vector3.down * 0.6f;
        if (map.WorldToCell(below, out int x, out int y) && map.GetTile(x, y) >= 0)
            map.SetTile(x, y, -1);   // empty: the collider updates too
    }
}
}
