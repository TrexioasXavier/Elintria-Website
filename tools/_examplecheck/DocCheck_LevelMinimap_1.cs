namespace DocCheck_LevelMinimap
{
using Elintria;
using Elintria.Standard;

public class EasyMode : ElintriaScript
{
    public bool easy = true;

    public override void Start()
    {
        foreach (LevelMinimap m in FindObjectsOfType<LevelMinimap>()) m.reveal = !easy;
    }
}
}
