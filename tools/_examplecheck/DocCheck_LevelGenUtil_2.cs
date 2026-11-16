namespace DocCheck_LevelGenUtil
{
using Elintria;

public class LastLevelInfo : ElintriaScript
{
    public override void Update()
    {
        GeneratedLevel l = LevelGenUtil.last;
        if (l != null && Input.GetKeyDown(KeyCode.I))
            Debug.Log("Seed " + l.seed + ", " + l.rooms + " rooms, start " + l.playerStart);
    }
}
}
