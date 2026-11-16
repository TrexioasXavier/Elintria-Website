namespace DocCheck_LevelGenUtil
{
using Elintria;

public class Restart : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        LevelGenUtil.DestroyLast();
        LevelGenerator.Generate("cave", "seed=" + Random.Range(1, 9999));
    }
}
}
