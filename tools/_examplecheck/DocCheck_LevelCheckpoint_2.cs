namespace DocCheck_LevelCheckpoint
{
using Elintria;
using Elintria.Standard;

public class StuckKey : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F5)) return;
        PlayerRespawn pr = GetComponent<PlayerRespawn>();
        if (pr != null) pr.Respawn();     // the last checkpoint, or the start
    }
}
}
