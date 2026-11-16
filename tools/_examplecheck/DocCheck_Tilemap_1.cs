namespace DocCheck_Tilemap
{
using Elintria;

public class TileGen : ElintriaScript
{
    public int width = 60;
    public int ground = 3;

    public override void Start()
    {
        Tilemap map = Tilemap.Get(gameObject);
        int height = 2;
        for (int x = 0; x < width; x++)
        {
            if (Random.value < 0.2f) height = Mathf.Clamp(height + Random.Range(-1, 2), 1, 6);
            for (int y = 0; y < height; y++) map.SetTile(x, y, ground);
        }
    }
}
}
