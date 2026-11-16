namespace DocCheck_LevelLightAnimator
{
using Elintria;

public class DarkLevel : ElintriaScript
{
    public override void Start()
    {
        // draw and colliders, but no Light objects and no fog: this script lights it itself
        GeneratedLevel level = LevelGenerator.Generate("cave, three spiders", "seed=4", Vector3.zero, -1, true, true, false);
        if (level != null) Debug.Log(level.Markers("Light").Count + " light spots left for my torches");
    }
}
}
