namespace DocCheck_DungeonResult
{
using Elintria;
using Elintria.Procedural;

public class StartEnd : ElintriaScript
{
    public GameObject player;
    public GameObject exit;
    public float cell = 2f;

    public override void Start()
    {
        DungeonResult d = Dungeon.Rooms(48, 48, 5);
        player.transform.position = new Vector3(d.start.x * cell, 1f, d.start.y * cell);
        exit.transform.position = new Vector3(d.end.x * cell, 0f, d.end.y * cell);
    }
}
}
