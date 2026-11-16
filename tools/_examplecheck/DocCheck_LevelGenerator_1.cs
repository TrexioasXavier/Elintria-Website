namespace DocCheck_LevelGenerator
{
using Elintria;

public class KeyHunt : ElintriaScript
{
    public GameObject keyTemplate;
    public GameObject bossTemplate;

    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("abandoned facility", "seed=12;mission=1;missionLocks=3;missionLockKind=0");
        if (level == null) return;
        level.PlaceCopies("Key", keyTemplate);
        level.PlaceCopies("MiniBoss", bossTemplate);
        Debug.Log(level.Markers("LockedDoor").Count + " locked doors on the way out");
    }
}
}
