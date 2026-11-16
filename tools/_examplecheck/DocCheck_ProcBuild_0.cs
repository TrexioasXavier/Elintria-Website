namespace DocCheck_ProcBuild
{
using Elintria;
using Elintria.Procedural;

public class OneCallDungeon : ElintriaScript
{
    public GameObject player;

    public override void Start()
    {
        int rooms = ProcBuild.BuildDungeon(Random.Range(0, 99999), 48, 48, 10, 2f, "FloorTile", "WallBlock", Vector3.zero, out Vector3 start);
        player.transform.position = start + Vector3.up;
        Debug.Log(rooms + " rooms built; the player starts at " + start);
    }
}
}
