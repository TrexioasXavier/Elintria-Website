namespace DocCheck_GeneratedLevel
{
using Elintria;

public class NextLevel : ElintriaScript
{
    public GameObject player;
    GeneratedLevel current;
    int number;

    public override void Start() { Load(); }

    public override void Update()
    {
        if (current != null && Vector3.Distance(player.transform.position, current.exit) < 2f) Load();
    }

    void Load()
    {
        if (current != null) current.Destroy();
        current = LevelGenerator.Generate("bunker", "seed=" + (++number));
        if (current != null) player.transform.position = current.playerStart + Vector3.up;
    }
}
}
