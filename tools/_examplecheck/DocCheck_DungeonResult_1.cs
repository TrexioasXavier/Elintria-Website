namespace DocCheck_DungeonResult
{
using Elintria;
using Elintria.Procedural;

public class DoorPlacer : ElintriaScript
{
    public GameObject doorTemplate;

    public override void Start()
    {
        DungeonResult d = Dungeon.Bsp(64, 64, 2);
        foreach (Cell c in d.doors)
            Instantiate(doorTemplate, new Vector3(c.x * 2f, 0f, c.y * 2f), Quaternion.identity).SetActive(true);
    }
}
}
