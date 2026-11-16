namespace DocCheck_LevelGenSettings
{
using Elintria;

public class Difficulty : ElintriaScript
{
    public int depth = 1;

    public GeneratedLevel Next()
    {
        var s = new LevelGenSettings("dark caves")
            .Seed(1000 + depth)
            .Set("enemies", 3 + depth * 2)
            .Set("loot", 4 + depth)
            .Set("boss", depth % 5 == 0);
        ++depth;
        return LevelGenerator.Generate(s);
    }
}
}
