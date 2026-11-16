namespace DocCheck_LevelGenerator
{
using Elintria;

public class RunStart : ElintriaScript
{
    public GameObject player;

    public override void Start()
    {
        int seed = (int)(System.DateTime.Now.Ticks % 100000);
        GeneratedLevel level = LevelGenerator.Generate("small dungeon, lots of loot", "seed=" + seed);
        if (level == null) { Debug.LogWarning("No level: " + LevelGenerator.lastError); return; }
        player.transform.position = level.playerStart + new Vector3(0f, 1f, 0f);
        Debug.Log("Seed " + seed + ": " + level.rooms + " rooms");
    }
}
}
