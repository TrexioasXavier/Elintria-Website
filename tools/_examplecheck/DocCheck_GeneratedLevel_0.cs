namespace DocCheck_GeneratedLevel
{
using Elintria;

public class Populate : ElintriaScript
{
    public GameObject zombie;
    public GameObject crate;

    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("hospital, eight zombies", "seed=5");
        if (level == null) return;
        int z = level.PlaceCopies("Enemy", zombie).Count;
        int c = level.PlaceCopies("Loot", crate).Count;
        Debug.Log(z + " zombies, " + c + " crates");
    }
}
}
