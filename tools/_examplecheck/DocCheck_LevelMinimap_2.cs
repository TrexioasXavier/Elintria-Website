namespace DocCheck_LevelMinimap
{
using Elintria;
using Elintria.Standard;

public class CutsceneHidesMap : ElintriaScript
{
    public void BeginCutscene() { foreach (LevelMinimap m in FindObjectsOfType<LevelMinimap>()) m.enabled = false; }
    public void EndCutscene() { foreach (LevelMinimap m in FindObjectsOfType<LevelMinimap>()) m.enabled = true; }
}
}
