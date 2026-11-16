namespace DocCheck_LevelCheckpoint
{
using Elintria;
using Elintria.Standard;

public class CheckpointCount : ElintriaScript
{
    public override void Update()
    {
        int got = 0, all = 0;
        foreach (LevelCheckpoint c in FindObjectsOfType<LevelCheckpoint>()) { ++all; if (c.reached) ++got; }
        ScreenGUI.Text(20f, 20f, "Checkpoints " + got + " / " + all);
    }
}
}
